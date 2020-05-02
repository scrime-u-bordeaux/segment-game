/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;

namespace SEGMent
{
	public enum TRANSITION_TYPE {NO_TYPE, CLIC_AREA_TYPE, SOLUTION_TYPE, OBJECT_STATE_SOLUTION_TYPE, PUZZLE_SOLUTION_TYPE, BACK_TRANSITION};

	public class GraphTransition
	{
		private int m_transitionID = -1;
		private TRANSITION_TYPE m_transitionType = TRANSITION_TYPE.NO_TYPE;

		private GraphNode m_nodeFrom = null;
		private GraphNode m_nodeTo = null;

		private List<string> m_transitionSounds; 

		private bool m_isImmediate = false;
		private bool m_isUnique = false;

		private bool m_hasBeenFiredOnce = false;

        private List<string> m_requieredEvents;
        private List<string> m_blockingEvents;
        private List<string> m_eventsToAdd;
        private List<string> m_eventsToRemove;



        public GraphTransition (GraphNode from, GraphNode to, bool isImmediate = false, bool isUnique = false)
		{
			m_nodeFrom = from;
			m_nodeTo = to;

			m_nodeFrom.AddOutgoingTransition(this);

			if (m_nodeTo != null) {
				m_nodeTo.AddIngoingTransition(this);
			}

			m_transitionSounds = new List<string>();

            m_requieredEvents = new List<string>();
            m_blockingEvents = new List<string>();
            m_eventsToAdd = new List<string>();
            m_eventsToRemove = new List<string>();

			m_isImmediate = isImmediate;
			m_isUnique = isUnique;
		}

		public void SetTransitionType(TRANSITION_TYPE transitionType) {
			m_transitionType = transitionType;
		}

		public TRANSITION_TYPE GetTransitionType() {
			return m_transitionType;
		}

		public void SetTransitionID(int id) {
			m_transitionID = id;
		}
		
		public int GetTransitionID() {
			return m_transitionID;
		}

		public bool IsActive() {
			return true;
		}

		public void AddTransitionSound(string soundName) {
			m_transitionSounds.Add(soundName);
		}

		public List<string> GetTransitionSound() {
			return m_transitionSounds;
		}

		public GraphNode GetNodeFrom() {
			return m_nodeFrom;
		}

		public GraphNode GetNodeTo() {
			return m_nodeTo;
		}

		public bool IsImmediate() {
			return m_isImmediate;
		}

		public bool IsUnique() {
			return m_isUnique;
		}

		public void TransitionIsFired() {
			m_hasBeenFiredOnce = true;
		}

		public bool ShouldBeAutomaticallyFired() {
			return (m_isUnique && m_hasBeenFiredOnce);
		}

        public List<string> GetRequieredEvents()
        {
            return m_requieredEvents;
        }

        public List<string> GetBlockingEvents()
        {
            return m_blockingEvents;
        }

        public List<string> GetEventsToAdd()
        {
            return m_eventsToAdd;
        }

        public List<string> GetEventsToRemove()
        {
            return m_eventsToRemove;
        }

        public void AddEventToAdd(string eventName)
        {
           // GenericLog.Log("EVENT TO ADD " + eventName);
            m_eventsToAdd.Add(eventName);
        }

        public void AddEventToRemove(string eventName)
        {
            // GenericLog.Log("EVENT TO REMOVE " + eventName);
            m_eventsToRemove.Add(eventName);
        }

        public void AddEventToCheck(string eventName)
        {
            if (eventName.Length > 0)
            {
                if (eventName[0] == '!')
                {
               //     GenericLog.Log("BLOCKING EVENT " + eventName.Substring(1));
                    m_blockingEvents.Add(eventName.Substring(1));
                } else
                {
                 //   GenericLog.Log("REQUIERED EVENT " + eventName);
                    m_requieredEvents.Add(eventName);
                }
            }
            
        }
    }
}

