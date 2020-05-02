﻿/* Author : Raphaël Marczak - 2016-2018 ; Vincent Casamayou - 2019
 *
 * SPDX-License-Identifier: AGPL-3.0-or-later
 *
 */

using System.Collections;
using System.Collections.Generic;

namespace SEGMent
{
	public class Player
	{
		string m_name;
		GameStructureRooms m_map;
		Room m_currentRoom;

		InformationManager m_informationManager;

		public Player (InformationManager informationManager)
		{
			m_name = "";
			m_map = null;
			m_informationManager = informationManager;

			m_currentRoom = null;
		}

		public void SetName(string playerName) {
			m_name = playerName;
		}

		public string GetName() {
			return m_name;
		}

		public void SetMap(GameStructureRooms map) 
		{
			m_map = map;
		}

		//Author - Vincent Casamayou - June 2019
 		//Get the state of the radar for the current Room
		public bool GetRadarInfo()
		{	
			if (m_currentRoom != null){
				return m_currentRoom.GetRadar();
			}
			return true;
		}

		public string GetCurrentRoomDescription ()
		{
			string resultString = "";

			if (m_currentRoom != null) {
				return m_currentRoom.GetDescription ();
			} 
		
			return resultString;
		}

		public string PopCurrentRoomDescription ()
		{
			string resultString = "";

			if (m_currentRoom != null) {
				return m_currentRoom.PopDescription ();
			} 

			return resultString;
		}

        public RoomClue PopCurrentRoomClue()
        {
            
            if (m_currentRoom != null)
            {
                return m_currentRoom.PopClue();
            }

            return null;
        }

        public string PopCurrentRoomDiaryEntryName ()
		{
			string resultString = "";

			if (m_currentRoom != null) {
				return m_currentRoom.PopDiaryEntry ();
			} 

			return resultString;
		}

		public bool ShouldDiaryEntryBeHighlighted() {
			if (m_currentRoom != null) {
				return m_currentRoom.ShouldDiaryEntryBeHighlighted();
			} 

			return false;
		}
		
		public string GetCurrentRoomBackgroundImageURL ()
		{
			string resultString = "";
				
			if (m_currentRoom != null) {
				return m_currentRoom.GetBackgroundImageURL ();
			} 

			return resultString;
		}
		
		public string GetCurrentRoomBackgroundMusicName ()
		{
			string resultString = "";
				
			if (m_currentRoom != null) {
				return m_currentRoom.GetBackgroundMusicName ();
			} 

			return resultString;
		}

		public bool MustCurrentRoomBackgroudMusicLoop() {
			if (m_currentRoom != null) {
				return m_currentRoom.MusicMustLoop();
			} 
			
			return false;
		}

		public List<int> GetListOfItemIndexes() {
			List<int> itemsID = new List<int>();
			List<Item> itemsInCurrentRoom = m_map.GetCurrentRoom().GetIncludedItems();
			
			foreach (Item currentItem in itemsInCurrentRoom) {
				itemsID.Add(currentItem.GetNodeID());
			}

			return itemsID;
		}

		public int GetCurrentRoomID() {
			return m_map.GetCurrentRoomID();
		}

		public List<int> GetListOfClickTextIndexes() {
			List<int> clickTextsID = new List<int>();
			List<ClickText> clickTextInCurrentRoom = m_map.GetCurrentRoom().GetIncludedClickText();
			
			foreach (ClickText currentClickText in clickTextInCurrentRoom) {
				clickTextsID.Add(currentClickText.GetNodeID());
			}
			
			return clickTextsID;
		}

		public bool Answer(string answer, ref string help) {
			return m_map.ReceiveAnswer(answer, ref help);
		}

		public bool ClicToMove(float x, float y) {
			return m_map.ClicToMove(x, y);
		}

		public bool ObjectStateSolutionValidated() {
			return m_map.ObjectStateSolutionValidated();
		}

		public bool PuzzleSolutionValidated() {
			return m_map.PuzzleSolutionValidated();
		}

		public List<BoundingBox> GetListOfDisplacementAreas() {
			return m_map.GetListOfDisplacementAreas();
		}

		public bool CanAnswer() {
			if (m_currentRoom != null) {
				return m_map.CanAnswer();
			}

			return false;
		}

    	public string GetInputQuestion() {
			if (m_currentRoom != null) {
				if (CanAnswer()) {
					return m_map.GetQuestion();
				}
			}

			return "";
		}

        // TODO : TO MAP !!!
        public bool GetInputIsPassword() {
			if (m_currentRoom != null) {
				if (CanAnswer()) {
					return m_map.GetIsSolutionPasswordType();
				}
			}

			return false;
		}

		public void Update() {
			if (m_map != null) {
				if (m_currentRoom != m_map.GetCurrentRoom()) {
					m_currentRoom = m_map.GetCurrentRoom();
					m_informationManager.AddPlayerModifInfo(PLAYER_MODIF_INFO.PLAYER_MOVED);
				}
			}
		}

		public InformationManager GetInformationManager() {
			return m_informationManager;
		}

		public GameStructureRooms GetMap() {
			return m_map;
		}

		public bool isRoomChangeImmediate() {
			return m_map.isRoomChangeImmediate();
		}

		public bool IsLeaveScene() {
			return m_map.IsLeaveRoom();
		}

		public List<Item> GetAllGameItems() {
			return m_map.getAllItems();
		}

	}
}
