﻿/* Author : Raphaël Marczak - 2016-2018 ; Vincent Casamayou - 2019
 *
 * SPDX-License-Identifier: AGPL-3.0-or-later
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SEGMent
{
    public class RoomClue
    {
        public string key;
        public List<string> clues;
    }

	public class Room: GraphNode
	{
		private string m_backgroundImageURL = "";
		private string m_backgroundSoundName = "";
		private string m_roomDescription = "";

		private bool m_radarOffState = true;

		private bool m_descriptionMustBeRepeated = false;

		private bool m_backgroundMusicMustLoop = true;

		private string m_diaryEntryName = "";
		private bool m_diaryEntryMustBeHighlighted;

		private List<Item> m_includedItems;
		private List<ClickText> m_includedClickText;
        private List<RoomClue> m_roomClues;


		public Room (): base()
		{
			SetNodeType(NODE_TYPE.ROOM_TYPE);

			m_includedItems = new List<Item>();
			m_includedClickText = new List<ClickText>();
            m_roomClues = new List<RoomClue>();
		}

		public bool IsAnAnswerRoom() 
		{
			foreach (GraphTransition transition in m_outgoingTransitions) {
				if ((transition.GetTransitionType() == TRANSITION_TYPE.SOLUTION_TYPE) && (transition.IsActive())) {
					return true;
				}
			}

			return false;
		}

		public string GetQuestion() 
		{
			foreach (GraphTransition transition in m_outgoingTransitions) {
				if ((transition.GetTransitionType() == TRANSITION_TYPE.SOLUTION_TYPE) && (transition.IsActive())) {
					return ((SolutionTransition) transition).GetQuestion();
				}
			}
			
			return "";
		}

		public bool GetIsSolutionPasswordType() {
			foreach (GraphTransition transition in m_outgoingTransitions) {
				if ((transition.GetTransitionType() == TRANSITION_TYPE.SOLUTION_TYPE) && (transition.IsActive())) {
					return ((SolutionTransition) transition).IsPassword();
				}
			}

			return false;
		}

		public void SetBackgroundImageURL (string url) {
			m_backgroundImageURL = url;
		}

		//Author - Vincent Casamayou - June 2019
 		//Update the Radar
		public void SetRadar(bool radar){
			m_radarOffState = radar;
		}

		//Author - Vincent Casamayou - June 2019
 		//Return the state of the radar for the pipeline
		public bool GetRadar()
		{
			return m_radarOffState;
		}

		public string GetBackgroundImageURL() {
			return m_backgroundImageURL;
		}

		public void SetBackgroundSoundName(string soundName, bool mustLoop) {
			m_backgroundSoundName = soundName;
			m_backgroundMusicMustLoop = mustLoop;
		}

		public string GetBackgroundMusicName() {
			return m_backgroundSoundName;
		}

		public void SetDescription(string description, bool mustBeRepeated = false) {
			m_roomDescription = description;
			m_descriptionMustBeRepeated = mustBeRepeated;
		}
		
		public string GetDescription() {
			return m_roomDescription;
		}

		public string PopDescription() {
			string result = m_roomDescription;

			if (!m_descriptionMustBeRepeated) {
				m_roomDescription = "";
			}

			return result;
		}

		public void SetDiaryEntry(string diaryName, bool mustBeHighlighted = false) {
			m_diaryEntryName = diaryName;
			m_diaryEntryMustBeHighlighted = mustBeHighlighted;
		}

        public void AddRoomClues(string key, List<string> values)
        {
            if (values.Count > 0)
            {
                RoomClue currentClue = new RoomClue();

                currentClue.key = key;
                currentClue.clues = new List<string>(values);

                m_roomClues.Add(currentClue);

               /* Debug.Log("ROOM CUE KEY " + currentClue.key);

                foreach (string currentValue in currentClue.clues)
                {
                    Debug.Log("ROOM CUE VALUE " + currentValue);
                }*/
            }
           
        }

        public RoomClue PopClue()
        {
            if (m_roomClues.Count == 0)
            {
                return null;
            } else
            {
                RoomClue clueToPop = m_roomClues[0];

                m_roomClues.RemoveAt(0);
                return clueToPop;
            }
           
        }


        public string PopDiaryEntry() {
			string result = m_diaryEntryName;

			m_diaryEntryName = "";

			return result;
		}

		public bool ShouldDiaryEntryBeHighlighted() {
			return m_diaryEntryMustBeHighlighted;
		}

		public void AddItem(Item itemToAdd) {
			m_includedItems.Add(itemToAdd);
		}

		public bool MusicMustLoop() {
			return m_backgroundMusicMustLoop;
		}

		public List<Item> GetIncludedItems() {
			return m_includedItems;
		}

		public void AddClickText(ClickText clickTextToAdd) {
			m_includedClickText.Add(clickTextToAdd);
		}
		
		public List<ClickText> GetIncludedClickText() {
			return m_includedClickText;
		}



	}
}
