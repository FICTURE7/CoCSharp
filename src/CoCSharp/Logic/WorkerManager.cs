using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Provides methods to manage workers.
    /// </summary>
    public class WorkerManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerManager"/> class.
        /// </summary>
        public WorkerManager()
        {
            _villageObjects = new List<VillageObject>();
        }

        private readonly List<VillageObject> _villageObjects;
        internal int _totalWorkers;

        /// <summary>
        /// Gets the total number of workers available.
        /// </summary>
        public int TotalWorkers => _totalWorkers;

        /// <summary>
        /// Gets the number of free workers available.
        /// </summary>
        public int FreeWorkers => _totalWorkers - _villageObjects.Count;

        /// <summary>
        /// Allocates a worker to the specified <see cref="VillageObject"/>.
        /// </summary>
        /// <param name="vilObj"><see cref="VillageObject"/> to allocate the worker to.</param>
        public void AllocateWorker(VillageObject vilObj)
        {
            if (vilObj == null)
                throw new ArgumentNullException(nameof(vilObj));

            if (!_villageObjects.Contains(vilObj))
            {
                _villageObjects.Add(vilObj);
            }
        }

        /// <summary>
        /// Deallocates a worker allocated to the specified <see cref="VillageObject"/>.
        /// </summary>
        /// <param name="vilObj"><see cref="VillageObject"/> to deallocate the worker.</param>
        public void DeallotateWorker(VillageObject vilObj)
        {
            if (vilObj == null)
                throw new ArgumentNullException(nameof(vilObj));

            var id = vilObj.Id;
            if (_villageObjects.Contains(vilObj))
            {
                _villageObjects.Remove(vilObj);
            }
        }

        public void FinishFastestTask(int ctick)
        {
            var obj = GetFastestTask();
            if (obj is Building)
            {
                var building = (Building)obj;
                Debug.Assert(building.IsConstructing);

                if (building.IsConstructing)
                    building.SpeedUpConstruction(ctick);
            }
            else if (obj is Trap)
            {
                var trap = (Trap)obj;
                Debug.Assert(trap.IsConstructing);

                if (trap.IsConstructing)
                    trap.SpeedUpConstruction(ctick);
            }
            else if (obj is Obstacle)
            {
                var obstacle = (Obstacle)obj;
                Debug.Assert(obstacle.IsClearing);

                if (obstacle.IsClearing)
                    obstacle.FinishClear(ctick);
            }
        }

        private VillageObject GetFastestTask()
        {
            var shortestDuration = TimeSpan.MaxValue;
            var result = default(VillageObject);
            for (int i = 0; i < _villageObjects.Count; i++)
            {
                var obj = _villageObjects[i];
                var duration = default(TimeSpan);

                if (obj is Building)
                {
                    var building = (Building)obj;
                    Debug.Assert(building.IsConstructing);

                    duration = building.ConstructionDuration;
                }
                else if (obj is Trap)
                {
                    var trap = (Trap)obj;
                    Debug.Assert(trap.IsConstructing);

                    duration = trap.ConstructionDuration;
                }
                else if (obj is Obstacle)
                {
                    var obstacle = (Obstacle)obj;
                    Debug.Assert(obstacle.IsClearing);

                    duration = obstacle.ClearDuration;
                }

                if (duration < shortestDuration)
                {
                    shortestDuration = duration;
                    result = obj;
                }
            }

            return result;
        }
    }
}
