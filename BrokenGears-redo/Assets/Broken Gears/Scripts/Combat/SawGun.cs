namespace BrokenGears.Combat {
    using Enemies;
    using UnityEngine;
    using UnityEngine.Events;

    public class SawGun : AWeaponizedTurret {
        [SerializeField] private Bone blade;
        [SerializeField] private SawBladeCollider sawBladeCollider;
        [SerializeField] private ParticleSystem[] sawParticles;

        protected override Transform AttackOrigin() => null;

        protected override void Update() {
            base.Update();

            if (blade.Origin && target != defaultTarget) {
                RotateBlade();
            }
        }

        protected override void LateUpdate() {
            base.LateUpdate();
            PlayFX();   
        }
        
        protected override void DoAttack() {
            AEnemy enemy = sawBladeCollider.Enemy;

            if (enemy) {
                enemy.DoDamage(damage);
            }
        }

        private void PlayFX() {
            AEnemy enemy = sawBladeCollider.Enemy;

            PlayAudio(enemy);
            PlayParticles(enemy);
        }

        private void PlayAudio(bool on) {
            if (on) {
                attackAudio.Play();
            } else {
                attackAudio.Stop();
            }
        }

        private void PlayParticles(bool on) {
            for (int i = 0; i < sawParticles.Length; i++) {
                ParticleSystem system = sawParticles[i];
                if (on && !system.isPlaying) {
                    sawParticles[i].Play();
                } else if (!on && !system.isPaused) {
                    sawParticles[i].Stop();
                }
            }
        }

        private void RotateBlade() {
            Vector3 direction = Vector3.one;

            direction.x *= IsAxisRotation(Axis.x);
            direction.y *= IsAxisRotation(Axis.y);
            direction.z *= IsAxisRotation(Axis.z);

            blade.Origin.Rotate(direction * Time.deltaTime * blade.TurnSpeed);
        }

        private int IsAxisRotation(Axis axis) {
            return blade.Axis == axis ? 1 : 0;
        }

        public override UnityEvent OnAttack() {
            throw new System.NotImplementedException();
        }
    }
}