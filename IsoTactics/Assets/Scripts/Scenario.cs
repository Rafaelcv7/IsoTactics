using IsoTactics.Abilities;

namespace IsoTactics
{
    public class Scenario
    {
        public float ScenarioValue;
        public Ability TargetAbility;
        public OverlayTile TargetTile;
        public OverlayTile PositionTile;
        public bool AutoAttack;
        
        public Scenario(float scenarioValue, Ability targetAbility, OverlayTile targetTile, OverlayTile positionTile, bool autoAttack)
        {
            ScenarioValue = scenarioValue;
            TargetAbility = targetAbility;
            TargetTile = targetTile;
            PositionTile = positionTile;
            AutoAttack = autoAttack;
        }

        public Scenario()
        {
            ScenarioValue = -10000;
            TargetAbility = null;
            TargetTile = null;
            PositionTile = null;
            AutoAttack = false;
        }
    }
    
}