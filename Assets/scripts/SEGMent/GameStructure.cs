﻿  /* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * 
 */

using System.Collections;
using System.Collections.Generic;

namespace SEGMent
{
	public class GameStructure
	{
		protected GraphNode m_token = null;

		protected List<GraphTransition> m_transitions;

		protected InformationManager m_informationManager;

		protected List<Room> m_roomsPile;

        private Dictionary<string, string> m_currentEvents;

        public GameStructure (InformationManager informationManager)
		{
			m_transitions = new List<GraphTransition>();
			m_roomsPile = new List<Room> ();
			m_informationManager = informationManager;

            m_currentEvents = new Dictionary<string, string>();
        }

		public void SetTransitionSound(int transitionID, string soundName) {
			if (transitionID >= m_transitions.Count) {
				return;
			}
			
			m_transitions[transitionID].AddTransitionSound(soundName);
		}

		public void FireTransition(GraphTransition transitionToFire) {
			transitionToFire.TransitionIsFired();

            ComputeTransitionEvent(transitionToFire);
		
			if (transitionToFire.GetNodeFrom() == m_token) {
				m_token = transitionToFire.GetNodeTo();

				if (m_token.GetNodeType() == NODE_TYPE.ROOM_TYPE) {
					Room currentRoom = (Room) m_token;

					List<GraphTransition> outGoingTransitions = currentRoom.GetOutgoingTransitions();
                    
					foreach (GraphTransition currentTransition in outGoingTransitions) {
						if (currentTransition.ShouldBeAutomaticallyFired()) {
							//	GenericLog.Log("heyaaaaaaa2");
							FireTransition(currentTransition);
							return;

						}
				}



				foreach (string soundName in transitionToFire.GetTransitionSound()) {
					m_informationManager.AddTransitionSoundsToPlay(soundName);
				}
			}


		}
		}

        public bool HasEvent(string eventName)
        {
            return m_currentEvents.ContainsKey(eventName);
        }

        public void AddEvent(string eventName)
        {
            if (!HasEvent(eventName))
            {
                m_currentEvents[eventName] = eventName;
            }
        }

        public void RemoveEvent(string eventName)
        {
            if (HasEvent(eventName))
            {
                m_currentEvents.Remove(eventName);
            }
        }

        public bool IsTransitionEventReady(GraphTransition transition)
        {
            foreach (string currentEvent in transition.GetRequieredEvents())
            {
                if (!HasEvent(currentEvent))
                {
                    return false;
                }
            }

            foreach (string currentEvent in transition.GetBlockingEvents())
            {
                if (HasEvent(currentEvent))
                {
                    return false;
                }
            }

            return true;
        }

        public void ComputeTransitionEvent(GraphTransition transition)
        {
           /* foreach (string currentEvent in transition.GetEventsToAdd())
            {
                AddEvent(currentEvent);
            }

            foreach (string currentEvent in transition.GetEventsToRemove())
            {
                RemoveEvent(currentEvent);
            }*/

            foreach (string currentEvent in transition.PopEventsToAdd())
            {
                AddEvent(currentEvent);
            }

            foreach (string currentEvent in transition.PopEventsToRemove())
            {
                RemoveEvent(currentEvent);
            }
        }
    }
}

