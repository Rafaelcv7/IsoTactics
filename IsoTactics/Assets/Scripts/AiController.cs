using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IsoTactics;
using IsoTactics.Enums;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public GameEvents onEndTurn;
    public GameEvents moveAlongPath;
    private List<Character> _enemyCharacters;
    private List<Character> _allyCharacters;
    private Character _activeAi;
    private Character _targetPlayer;
    private PathFinder _pathFinder;
    private RangeFinder _rangeFinder;
    private List<OverlayTile> _path;
    private List<OverlayTile> _inRangeTiles;
    private OverlayTile _closestTile;
    private Scenario _bestScenario;
    private int _distance;
    
    // Start is called before the first frame update
    void Start()
    {
        _pathFinder = new PathFinder();
        _rangeFinder = new RangeFinder();
        _path = new List<OverlayTile>();
    }
    

    public void ExecuteTurn(Character activeAi)
    {
        _enemyCharacters = FindObjectsOfType<Character>().Where(x => x.teamId != activeAi.teamId && x.isAlive).ToList();
        _allyCharacters =  FindObjectsOfType<Character>().Where(x => x.teamId == activeAi.teamId && x.isAlive).ToList();
        _activeAi = activeAi;
        StartCoroutine(CalculateBestScenario());
    }

    private IEnumerator CalculateBestScenario()
    {
        var inRangeTiles = _rangeFinder.GetTilesInRange(_activeAi.activeTile.Grid2DLocation, _activeAi.Stats.GetStat(Stats.MovementPoints).statValue);

        var scenario = new Scenario();
        _distance = 1000;
        foreach (var tile in inRangeTiles)
        {
            if (!tile.isBlocked)
            {
                var tempScenario = CalculateTileScenarioValue(tile);
                scenario = CompareScenarios(scenario, tempScenario);
                scenario = GetClosestEqualScenario(inRangeTiles, scenario, tempScenario);
                scenario = CheckScenarioValueIfNoTarget(scenario, tile, tempScenario);
            }
        }

        if (scenario.PositionTile)
        {
            ApplyBestScenario(scenario);
        }
        else
        {
            StartCoroutine(EndTurn());
        }
        
        yield return null;
    }

    private IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(0.25f);
        Debug.Log($"{_activeAi.Name} ended his turn.");
        
        onEndTurn.Raise(this, null);
    }

    private Scenario CheckScenarioValueIfNoTarget(Scenario scenario, OverlayTile tile, Scenario tempScenario)
    {
        if (tempScenario.PositionTile == null && !scenario.TargetTile)
        {
            var targetCharacter = FindClosestCharacter(tile);
            if (targetCharacter)
            {
                var targetTile = GetClosestNeighbour(targetCharacter.activeTile);

                if (targetCharacter && targetTile)
                {
                    var pathToCharacter = _pathFinder.FindPath(tile, targetTile, new List<OverlayTile>());
                    var pathDistance = pathToCharacter.Count;

                    var scenarioValue = -pathDistance - targetCharacter.HP.currentHealth;
                    if (pathDistance < _distance)
                    {
                        if (tile.Grid2DLocation != _activeAi.activeTile.Grid2DLocation &&
                            tile.Grid2DLocation != targetCharacter.activeTile.Grid2DLocation &&
                            (scenarioValue > scenario.ScenarioValue || !scenario.PositionTile))
                        {
                            _distance = pathDistance;
                            scenario = new Scenario(scenarioValue, null, null, tile, false);
                        }
                    }
                }
            }
        }

        return scenario;
    }

    //if the new scenario and the current best scenario are equal, then take the closest scenario.
    private Scenario GetClosestEqualScenario(List<OverlayTile> inRangeTiles, Scenario scenario, Scenario tempScenario)
    {
        if (tempScenario.PositionTile != null && tempScenario.ScenarioValue == scenario.ScenarioValue)
        {
            var tempScenarioPathCount = _pathFinder.FindPath(_activeAi.activeTile, tempScenario.PositionTile, inRangeTiles).Count;
            var scenarioPathCount = _pathFinder.FindPath(_activeAi.activeTile, scenario.PositionTile, inRangeTiles).Count;

            if (tempScenarioPathCount < scenarioPathCount)
                scenario = tempScenario;
        }

        return scenario;
    }

    private Scenario CompareScenarios(Scenario scenario, Scenario tempScenario)
    {
        if ((tempScenario != null && tempScenario.ScenarioValue > scenario.ScenarioValue))
        {
            scenario = tempScenario;
        }

        return scenario;
    }

    private Scenario CalculateTileScenarioValue(OverlayTile tile)
    {
        switch (_activeAi.behaviour)
        {
            case Behaviours.CloseRange:
                return BasicAttack(tile, false, FindClosestCharacter(tile));
            case Behaviours.LongRange:
                return BasicAttack(tile, false, FindClosestCharacter(tile));
            case Behaviours.LongRangeExecutor:
                return BasicAttack(tile, true, FindLowestHealthCharacter(tile, false));
            default:
                return new Scenario();
        }

        //TODO: Check for abilities?
    }

    private Scenario BasicAttack(OverlayTile tile, bool range, Character targetCharacter)
    {

        if (targetCharacter)
        {
            var closestDistance = _pathFinder.GetManhattenDistance(tile, targetCharacter.activeTile);
            
            //Check if the closest character is in attack range and make sure we're not on the characters tile. 
            if (closestDistance <= _activeAi.abilities[0].abilityRange && tile != targetCharacter.activeTile)
            {

                var scenarioValue = 0;
                scenarioValue += range 
                    ? _activeAi.Stats.strength.statValue + (closestDistance - _activeAi.Stats.movementPoints.statValue)
                    : _activeAi.Stats.strength.statValue + (_activeAi.Stats.movementPoints.statValue - closestDistance);

                return new Scenario(scenarioValue, null, targetCharacter.activeTile, tile, true);
            }
        }

        return new Scenario();
    }

    private OverlayTile GetClosestNeighbour(OverlayTile targetCharacterTile)
    {
        var targetNeighbours = MapManager.Instance.GetSurroundingTiles(targetCharacterTile, new List<OverlayTile>());
        if (targetNeighbours.Count == 0) return null;
        var targetTile = targetNeighbours[0];
        var targetDistance = _pathFinder.GetManhattenDistance(targetTile, _activeAi.activeTile);

        foreach (var neighbour in targetNeighbours)
        {
            var distance = _pathFinder.GetManhattenDistance(neighbour, _activeAi.activeTile);

            if (distance < targetDistance)
            {
                targetTile = neighbour;
                targetDistance = distance;
            } 
        }

        return targetTile;
    }

    private Character FindClosestCharacter(OverlayTile tile)
    {
        Character targetCharacter = null;

        var closestDistance = 1000;
        foreach (var player in _enemyCharacters)
        {
            var currentDistance = _pathFinder.GetManhattenDistance(tile, player.activeTile);

            if (currentDistance <= closestDistance)
            {
                closestDistance = currentDistance;
                targetCharacter = player;
            }
        }

        return targetCharacter;
    }

    private Character FindLowestHealthCharacter(OverlayTile tile, bool isAlly)
    {
        return isAlly
            ? _allyCharacters.Aggregate((i1, i2) => i1.HP.currentHealth < i2.HP.currentHealth ? i1 : i2)
            : _enemyCharacters.Aggregate((i1, i2) => i1.HP.currentHealth < i2.HP.currentHealth ? i1 : i2);
    }

    //ApplyBest Scenario:
    private void ApplyBestScenario(Scenario scenario)
    {
        _bestScenario = scenario;
        var currentTile = _activeAi.activeTile;
        _path = _pathFinder.FindPath(currentTile, _bestScenario.PositionTile, new List<OverlayTile>());
        
        //If it can attack but it doesn't need to move, attack
        //TODO: Establish behaviour from the start and develop logic after that.
        if ((_path.Count == 0 || IsStandingNextToTarget(_bestScenario.TargetTile)) && _bestScenario.TargetTile != null && _activeAi.behaviour == Behaviours.CloseRange)
        {
            Invoke(nameof(Attack), 1f);
        }
        else
        {
            //Moving
            StartCoroutine(Move(_path));
            if (_bestScenario != null && (_bestScenario.TargetTile != null || _bestScenario.TargetAbility != null))
                Invoke(nameof(Attack), 1f);
            else
                StartCoroutine(EndTurn());
        }
    }
    

    private void Attack()
    {
        if (_bestScenario.AutoAttack && _bestScenario.TargetTile.activeCharacter)
        {
            StartCoroutine(AttackTargetCharacter(_bestScenario.TargetTile.activeCharacter));
        }
    }

    private IEnumerator AttackTargetCharacter(Character targetCharacter)
    {
        targetCharacter.activeTile.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(0.5f);
        
        Debug.Log($"{_activeAi.Name} dealt damage to {targetCharacter.Name}");
        
        //For now use basic attack ability.
        _activeAi.State.EvaluateMovingState(targetCharacter.activeTile, false);
        _activeAi.abilities[0].Execute(targetCharacter.activeTile);
        _activeAi.Stats.actionPoints.statValue--;
        targetCharacter.activeTile.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        StartCoroutine(EndTurn());
    }

    private IEnumerator Move(List<OverlayTile> path)
    {
        yield return new WaitForSeconds(0.25f);
        moveAlongPath.Raise(this, path);
        yield return new WaitForSeconds(1f);
    }

    private bool IsStandingNextToTarget(OverlayTile targetCharacterTile)
    {
        var targetNeighbours = MapManager.Instance.GetSurroundingTiles(targetCharacterTile, new List<OverlayTile>(), true);
        return targetNeighbours.Any(x => x == _activeAi.activeTile);
    }
}
