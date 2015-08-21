namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from particle_emitters.csv.
    /// </summary>
    public class ParticleEmitterData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="ParticleEmitterData"/> class.
        /// </summary>
        public ParticleEmitterData()
        {
            // Space
        }

        public string Name { get; set; }
        public int ParticleCount { get; set; }
        public int EmissionTime { get; set; }
        public int MinLife { get; set; }
        public int MaxLife { get; set; }
        public int MinHorizAngle { get; set; }
        public int MaxHorizAngle { get; set; }
        public int MinVertAngle { get; set; }
        public int MaxVertAngle { get; set; }
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public int StartZ { get; set; }
        public int Gravity { get; set; }
        public int Inertia { get; set; }
        public int StartScale { get; set; }
        public int EndScale { get; set; }
        public int MinRotate { get; set; }
        public int MaxRotate { get; set; }
        public int ParticleFadeOutTime { get; set; }
        public int StartRadius { get; set; }
        public string ParticleSwf { get; set; }
        public string ParticleExportName { get; set; }
        public bool ScaleTimeline { get; set; }
        public bool OrientToMovement { get; set; }
        public bool OrientToParent { get; set; }
        public bool BounceFromGround { get; set; }
        public bool AdditiveBlend { get; set; }
        public int FadeInTime { get; set; }
        public int FadeOutTime { get; set; }
        public bool IsIsoParticle { get; set; }
        public int StartAngleMin { get; set; }
        public int StartAngleMax { get; set; }
        public int ScaleRandomMin { get; set; }
        public int ScaleRandomMax { get; set; }
    }
}
