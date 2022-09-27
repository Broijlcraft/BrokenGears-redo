namespace BrokenGears.Enemies {

    public class DefaultTarget : AEnemy {
        protected override float DefaultHealth() => 0;
        public override HealthEvent Events() => null;
    }
}