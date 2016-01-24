using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans avatar.
    /// </summary>
    public class Avatar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Avatar"/> class.
        /// </summary>
        public Avatar()
        {
            _level = 1;
        }

        /// <summary>
        /// Gets or sets the username of the <see cref="Avatar"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether the <see cref="Avatar"/> has been named.
        /// </summary>
        public bool IsNamed { get; set; }

        /// <summary>
        /// Gets or sets the user token of the <see cref="Avatar"/>.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the user ID of the <see cref="Avatar"/>.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Gets the shield duration of the <see cref="Avatar"/>.
        /// </summary>
        public TimeSpan ShieldDuration
        {
            get
            {
                return TimeSpan.FromSeconds(DateTimeConverter.ToUnixTimestamp(ShieldEndTime) - DateTimeConverter.UtcNow);
            }
        }

        /// <summary>
        /// Gets or sets the shield UTC end time of the <see cref="Avatar"/>.
        /// </summary>
        public DateTime ShieldEndTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Village"/> associated with this
        /// <see cref="Avatar"/>.
        /// </summary>
        public Village Home { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Clan"/> associated with this
        /// <see cref="Avatar"/>.
        /// </summary>
        public Clan Alliance { get; set; }

        /// <summary>
        /// Gets or sets the league of the <see cref="Avatar"/>.
        /// </summary>
        public int League { get; set; }

        /// <summary>
        /// Gets or sets the level of the <see cref="Avatar"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Level is less than 1.</exception>
        public int Level
        {
            get { return _level; }
            set
            {
                if (value < 1) // CoC crashes when lvl is less than 1
                    throw new ArgumentOutOfRangeException("Level cannot be less than 1.");
                _level = value;
            }
        }
        private int _level;

        /// <summary>
        /// Gets or sets the experience of the <see cref="Avatar"/>.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Gets or sets the amount of gems of the <see cref="Avatar"/>.
        /// </summary>
        public int Gems { get; set; }

        /// <summary>
        /// Gets or sets the amount of free gems of the <see cref="Avatar"/>.
        /// </summary>
        public int FreeGems { get; set; }

        /// <summary>
        /// Gets or sets the amount of trophies of the <see cref="Avatar"/>.
        /// </summary>
        public int Trophies { get; set; }

        /// <summary>
        /// Gets or sets the number of attacks won.
        /// </summary>
        public int AttacksWon { get; set; }

        /// <summary>
        /// Gets or sets the number of attacks lost.
        /// </summary>
        public int AttacksLost { get; set; }

        /// <summary>
        /// Gets or sets the number of defenses lost.
        /// </summary>
        public int DefensesWon { get; set; }

        /// <summary>
        /// Gets or sets the number of defenses lost.
        /// </summary>
        public int DefensesLost { get; set; }
    }
}
