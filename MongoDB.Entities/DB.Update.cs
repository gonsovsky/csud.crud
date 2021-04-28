﻿using MongoDB.Driver;

namespace MongoDB.Entities
{
    public static partial class DB
    {
        /// <summary>
        /// Represents an update command
        /// <para>TIP: Specify a filter first with the .Match() method. Then set property values with .Modify() and finally call .Execute() to run the command.</para>
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        /// <param name="session">An optional session if using within a transaction</param>
        public static Update<T> Update<T>(IClientSessionHandle session = null) where T : IEntity
            => new Update<T>(session);

        /// <summary>
        /// Update and retrieve the first document that was updated.
        /// <para>TIP: Specify a filter first with the .Match(). Then set property values with .Modify() and finally call .Execute() to run the command.</para>
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        /// <typeparam name="TProjection">The type to project to</typeparam>
        /// <param name="session">An optional session if using within a transaction</param>
        public static UpdateAndGet<T, TProjection> UpdateAndGet<T, TProjection>(IClientSessionHandle session = null) where T : IEntity
            => new UpdateAndGet<T, TProjection>(session);

        /// <summary>
        /// Update and retrieve the first document that was updated.
        /// <para>TIP: Specify a filter first with the .Match(). Then set property values with .Modify() and finally call .Execute() to run the command.</para>
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        /// <param name="session">An optional session if using within a transaction</param>
        public static UpdateAndGet<T> UpdateAndGet<T>(IClientSessionHandle session = null) where T : IEntity
            => new UpdateAndGet<T>(session);
    }
}
