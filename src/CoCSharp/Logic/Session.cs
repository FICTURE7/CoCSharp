using CoCSharp.Network.Cryptography;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a game session.
    /// </summary>
    public abstract class Session
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        protected Session()
        {
            _id = Crypto8.GenerateNonce();
#if DEBUG
            _ident = InternalUtils.BytesToString(_id);
#endif
        }
        #endregion

        #region Fields & Properties
        // Level which is associated with this session.
        private Level _level;
        // State of the session.
        private SessionState _state;
        // Session ID of the session.
        private readonly byte[] _id;

#if DEBUG
        private string _ident;
#endif

        /// <summary>
        /// Gets the byte array representing this session's identifier.
        /// </summary>
        public byte[] Id => _id;

        /// <summary>
        /// Gets the <see cref="Logic.Level"/> associated with this <see cref="Session"/>.
        /// </summary>
        public Level Level => _level;

        /// <summary>
        /// Gets the <see cref="SessionState"/> of the current session.
        /// </summary>
        public SessionState State => _state;
        #endregion

        #region Methods
        /// <summary>
        /// Associates the <see cref="Session"/> with the specified <see cref="Logic.Level"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <exception cref="InvalidOperationException"><see cref="Session"/> is already logged in.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        public void Login(Level level)
        {
            if (_level != null)
                throw new InvalidOperationException("Already logged in.");

            if (level == null)
                throw new ArgumentNullException(nameof(level));

            _level = level;
            _state = SessionState.LoggedIn;
            level.LoginCount++;

#if DEBUG
            _ident = _level.Token + _ident;
#endif
        }

        /// <summary>
        /// Searches for an opponent.
        /// </summary>
        /// <returns>An opponent.</returns>
        public abstract Level SearchOpponent();
        #endregion
    }
}
