﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BizHawk.Client.Common
{
	public class TasMovieChangeLog
	{
		public TasMovieChangeLog(TasMovie movie)
		{
			_history = new List<List<IMovieAction>>();
			_movie = movie;
		}

		private readonly List<List<IMovieAction>> _history = new List<List<IMovieAction>>();
		private readonly TasMovie _movie;

		private int _maxSteps = 100;
		private int _totalSteps;
		private bool _recordingBatch;

		public List<string> Names { get; } = new List<string>();
		public int UndoIndex { get; private set; } = -1;

		public int MaxSteps
		{
			get
			{
				return _maxSteps;
			}

			set
			{
				_maxSteps = value;
				if (_history.Count > value)
				{
					if (_history.Count <= value)
					{
						ClearLog();
					}
					else
					{
						ClearLog(_history.Count - value);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the movie is in recording mode
		/// This is not intended to turn off the ChangeLog, but to disable the normal recording process.
		/// Use this to manually control the ChangeLog. (Useful for when you are making lots of 
		/// </summary>
		public bool IsRecording { get; set; } = true;

		public void ClearLog(int upTo = -1)
		{
			if (upTo == -1)
			{
				upTo = _history.Count;
			}

			_history.RemoveRange(0, upTo);
			Names.RemoveRange(0, upTo);
			UndoIndex -= upTo;
			if (UndoIndex < -1)
			{
				UndoIndex = -1;
			}

			if (_history.Count == 0)
			{
				_recordingBatch = false;
			}
		}

		private void TruncateLog(int from)
		{
			_history.RemoveRange(from, _history.Count - from);
			Names.RemoveRange(from, Names.Count - from);

			if (UndoIndex < _history.Count - 1)
			{
				UndoIndex = _history.Count - 1;
			}

			if (_recordingBatch)
			{
				_recordingBatch = false;
				BeginNewBatch();
			}
		}

		/// <summary>
		/// All changes made between calling Begin and End will be one Undo.
		/// If already recording in a batch, calls EndBatch.
		/// </summary>
		/// <param name="name">The name of the batch</param>
		/// <param name="keepOldBatch">If set and a batch is in progress, a new batch will not be created.</param>
		/// <returns>Returns true if a new batch was started; otherwise false.</returns>
		public bool BeginNewBatch(string name = "", bool keepOldBatch = false)
		{
			if (!IsRecording)
			{
				return false;
			}

			bool ret = true;
			if (_recordingBatch)
			{
				if (keepOldBatch)
				{
					ret = false;
				}
				else
				{
					EndBatch();
				}
			}

			if (ret)
			{
				ret = AddMovieAction(name);
			}

			_recordingBatch = true;

			return ret;
		}

		/// <summary>
		/// Ends the current undo batch. Future changes will be one undo each.
		/// If not already recording a batch, does nothing.
		/// </summary>
		public void EndBatch()
		{
			if (!IsRecording || !_recordingBatch)
			{
				return;
			}

			_recordingBatch = false;
			List<IMovieAction> last = _history.Last();
			if (last.Count == 0) // Remove batch if it's empty.
			{
				_history.RemoveAt(_history.Count - 1);
				Names.RemoveAt(Names.Count - 1);
				UndoIndex--;
			}
			else
			{
				last.Capacity = last.Count;
			}
		}

		/// <summary>
		/// Undoes the most recent action batch, if any exist.
		/// </summary>
		/// <returns>Returns the frame which the movie needs to rewind to.</returns>
		public int Undo()
		{
			if (UndoIndex == -1)
			{
				return _movie.InputLogLength;
			}

			List<IMovieAction> batch = _history[UndoIndex];
			for (int i = batch.Count - 1; i >= 0; i--)
			{
				batch[i].Undo(_movie);
			}

			UndoIndex--;

			_recordingBatch = false;

			if (batch.All(a => a.GetType() == typeof(MovieActionMarker)))
			{
				return _movie.InputLogLength;
			}

			return PreviousUndoFrame;
		}

		/// <summary>
		/// Redoes the most recent undo, if any exist.
		/// </summary>
		/// <returns>Returns the frame which the movie needs to rewind to.</returns>
		public int Redo()
		{
			if (UndoIndex == _history.Count - 1)
			{
				return _movie.InputLogLength;
			}

			UndoIndex++;
			List<IMovieAction> batch = _history[UndoIndex];
			foreach (IMovieAction b in batch)
			{
				b.Redo(_movie);
			}

			_recordingBatch = false;

			if (batch.All(a => a.GetType() == typeof(MovieActionMarker)))
			{
				return _movie.InputLogLength;
			}

			return PreviousRedoFrame;
		}

		public bool CanUndo => UndoIndex > -1;
		public bool CanRedo => UndoIndex < _history.Count - 1;

		public string NextUndoStepName
		{
			get
			{
				if (Names.Count == 0 || UndoIndex < 0)
				{
					return null;
				}

				return Names[UndoIndex];
			}
		}

		public int PreviousUndoFrame
		{
			get
			{
				if (UndoIndex == _history.Count - 1)
				{
					return _movie.InputLogLength;
				}

				if (_history[UndoIndex + 1].Count == 0)
				{
					return _movie.InputLogLength;
				}

				return _history[UndoIndex + 1].Min(a => a.FirstFrame);
			}
		}

		public int PreviousRedoFrame
		{
			get
			{
				if (UndoIndex == -1)
				{
					return _movie.InputLogLength;
				}

				if (_history[UndoIndex].Count == 0)
				{
					return _movie.InputLogLength;
				}

				return _history[UndoIndex].Min(a => a.FirstFrame);
			}
		}

		#region Change History

		private bool AddMovieAction(string name)
		{
			if (UndoIndex + 1 != _history.Count)
			{
				TruncateLog(UndoIndex + 1);
			}

			if (name == "")
			{
				name = "Undo step " + _totalSteps;
			}

			bool ret = false;
			if (!_recordingBatch)
			{
				ret = true;
				_history.Add(new List<IMovieAction>(1));
				Names.Add(name);
				_totalSteps += 1;

				if (_history.Count <= MaxSteps)
				{
					UndoIndex += 1;
				}
				else
				{
					_history.RemoveAt(0);
					Names.RemoveAt(0);
					ret = false;
				}
			}

			return ret;
		}

		public void SetName(string name)
		{
			Names[Names.Count - 1] = name;
		}

		// TODO: These probably aren't the best way to handle undo/redo.
		private int _lastGeneral;

		public void AddGeneralUndo(int first, int last, string name = "", bool force = false)
		{
			if (IsRecording || force)
			{
				AddMovieAction(name);
				_history.Last().Add(new MovieAction(first, last, _movie));
				_lastGeneral = _history.Last().Count - 1;
			}
		}

		public void SetGeneralRedo(bool force = false)
		{
			if (IsRecording || force)
			{
				(_history.Last()[_lastGeneral] as MovieAction).SetRedoLog(_movie);
			}
		}

		public void AddBoolToggle(int frame, string button, bool oldState, string name = "", bool force = false)
		{
			if (IsRecording || force)
			{
				AddMovieAction(name);
				_history.Last().Add(new MovieActionFrameEdit(frame, button, oldState, !oldState));
			}
		}

		public void AddFloatChange(int frame, string button, float oldState, float newState, string name = "", bool force = false)
		{
			if (IsRecording || force)
			{
				AddMovieAction(name);
				_history.Last().Add(new MovieActionFrameEdit(frame, button, oldState, newState));
			}
		}

		public void AddMarkerChange(TasMovieMarker newMarker, int oldPosition = -1, string oldMessage = "", string name = "", bool force = false)
		{
			if (IsRecording || force)
			{
				if (oldPosition == -1)
				{
					name = "Set Marker at frame " + newMarker.Frame;
				}
				else
				{
					name = "Remove Marker at frame " + oldPosition;
				}

				AddMovieAction(name);
				_history.Last().Add(new MovieActionMarker(newMarker, oldPosition, oldMessage));
			}
		}

		public void AddInputBind(int frame, bool isDelete, string name = "", bool force = false)
		{
			if (IsRecording || force)
			{
				AddMovieAction(name);
				_history.Last().Add(new MovieActionBindInput(_movie, frame, isDelete));
			}
		}

		#endregion
	}

	#region Classes

	public interface IMovieAction
	{
		void Undo(TasMovie movie);
		void Redo(TasMovie movie);

		int FirstFrame { get; }
		int LastFrame { get; }
	}

	public class MovieAction : IMovieAction
	{
		public int FirstFrame { get; }
		public int LastFrame { get; }

		private readonly int _undoLength;
		private int _redoLength;

		private int Length => LastFrame - FirstFrame + 1;

		private readonly List<string> _oldLog;
		private List<string> _newLog;
		private readonly bool _bindMarkers;

		public MovieAction(int firstFrame, int lastFrame, TasMovie movie)
		{
			FirstFrame = firstFrame;
			LastFrame = lastFrame;
			_oldLog = new List<string>(Length);
			_undoLength = Math.Min(LastFrame + 1, movie.InputLogLength) - FirstFrame;

			for (int i = 0; i < _undoLength; i++)
			{
				_oldLog.Add(movie.GetLogEntries()[FirstFrame + i]);
			}

			_bindMarkers = movie.BindMarkersToInput;
		}

		public void SetRedoLog(TasMovie movie)
		{
			_redoLength = Math.Min(LastFrame + 1, movie.InputLogLength) - FirstFrame;
			_newLog = new List<string>();
			for (int i = 0; i < _redoLength; i++)
			{
				_newLog.Add(movie.GetLogEntries()[FirstFrame + i]);
			}
		}

		public void Undo(TasMovie movie)
		{
			bool wasRecording = movie.ChangeLog.IsRecording;
			bool wasBinding = movie.BindMarkersToInput;
			movie.ChangeLog.IsRecording = false;
			movie.BindMarkersToInput = _bindMarkers;

			if (_redoLength != Length)
			{
				movie.InsertEmptyFrame(FirstFrame, Length - _redoLength, true);
			}

			if (_undoLength != Length)
			{
				movie.RemoveFrames(FirstFrame, movie.InputLogLength - _undoLength, true);
			}

			for (int i = 0; i < _undoLength; i++)
			{
				movie.SetFrame(FirstFrame + i, _oldLog[i]);
			}

			movie.ChangeLog.IsRecording = wasRecording;
			movie.BindMarkersToInput = _bindMarkers;
		}

		public void Redo(TasMovie movie)
		{
			bool wasRecording = movie.ChangeLog.IsRecording;
			bool wasBinding = movie.BindMarkersToInput;
			movie.ChangeLog.IsRecording = false;
			movie.BindMarkersToInput = _bindMarkers;

			if (_undoLength != Length)
			{
				movie.InsertEmptyFrame(FirstFrame, Length - _undoLength);
			}

			if (_redoLength != Length)
			{
				movie.RemoveFrames(FirstFrame, movie.InputLogLength - _redoLength);
			}

			for (int i = 0; i < _redoLength; i++)
			{
				movie.SetFrame(FirstFrame + i, _newLog[i]);
			}

			movie.ChangeLog.IsRecording = wasRecording;
			movie.BindMarkersToInput = _bindMarkers;
		}
	}

	public class MovieActionMarker : IMovieAction
	{
		public int FirstFrame { get; }
		public int LastFrame { get; }

		private readonly string _oldMessage;
		private readonly string _newMessage;

		public MovieActionMarker(TasMovieMarker marker, int oldPosition = -1, string oldMessage = "")
		{
			FirstFrame = oldPosition;
			if (marker == null)
			{
				LastFrame = -1;
				_oldMessage = oldMessage;
			}
			else
			{
				LastFrame = marker.Frame;
				_oldMessage = oldMessage == "" ? marker.Message : oldMessage;
				_newMessage = marker.Message;
			}
		}

		public void Undo(TasMovie movie)
		{
			if (FirstFrame == -1) // Action: Place marker
			{
				movie.Markers.Remove(movie.Markers.Get(LastFrame), true);
			}
			else if (LastFrame == -1) // Action: Remove marker
			{
				movie.Markers.Add(FirstFrame, _oldMessage, true);
			}
			else // Action: Move/rename marker
			{
				movie.Markers.Move(LastFrame, FirstFrame, true);
				movie.Markers.Get(LastFrame).Message = _oldMessage;
			}
		}

		public void Redo(TasMovie movie)
		{
			if (FirstFrame == -1) // Action: Place marker
			{
				movie.Markers.Add(LastFrame, _oldMessage, true);
			}
			else if (LastFrame == -1) // Action: Remove marker
			{
				movie.Markers.Remove(movie.Markers.Get(FirstFrame), true);
			}
			else // Action: Move/rename marker
			{
				movie.Markers.Move(FirstFrame, LastFrame, true);
				movie.Markers.Get(LastFrame).Message = _newMessage;
			}
		}
	}

	public class MovieActionFrameEdit : IMovieAction
	{
		public int FirstFrame { get; }
		public int LastFrame => FirstFrame;

		private readonly float _oldState;
		private readonly float _newState;

		private readonly string _buttonName;
		private readonly bool _isFloat;

		public MovieActionFrameEdit(int frame, string button, bool oldS, bool newS)
		{
			_oldState = oldS ? 1 : 0;
			_newState = newS ? 1 : 0;
			FirstFrame = frame;
			_buttonName = button;
		}

		public MovieActionFrameEdit(int frame, string button, float oldS, float newS)
		{
			_oldState = oldS;
			_newState = newS;
			FirstFrame = frame;
			_buttonName = button;
			_isFloat = true;
		}

		public void Undo(TasMovie movie)
		{
			bool wasRecording = movie.ChangeLog.IsRecording;
			movie.ChangeLog.IsRecording = false;

			if (_isFloat)
			{
				movie.SetFloatState(FirstFrame, _buttonName, _oldState);
			}
			else
			{
				movie.SetBoolState(FirstFrame, _buttonName, _oldState == 1);
			}

			movie.ChangeLog.IsRecording = wasRecording;
		}

		public void Redo(TasMovie movie)
		{
			bool wasRecording = movie.ChangeLog.IsRecording;
			movie.ChangeLog.IsRecording = false;

			if (_isFloat)
			{
				movie.SetFloatState(FirstFrame, _buttonName, _newState);
			}
			else
			{
				movie.SetBoolState(FirstFrame, _buttonName, _newState == 1);
			}

			movie.ChangeLog.IsRecording = wasRecording;
		}
	}

	public class MovieActionPaint : IMovieAction
	{
		public int FirstFrame { get; }
		public int LastFrame { get; }
		private readonly List<float> _oldState;
		private readonly float _newState;
		private readonly string _buttonName;
		private readonly bool _isFloat = false;

		public MovieActionPaint(int startFrame, int endFrame, string button, bool newS, TasMovie movie)
		{
			_newState = newS ? 1 : 0;
			FirstFrame = startFrame;
			LastFrame = endFrame;
			_buttonName = button;
			_oldState = new List<float>(endFrame - startFrame + 1);

			for (int i = 0; i < endFrame - startFrame + 1; i++)
			{
				_oldState.Add(movie.BoolIsPressed(startFrame + i, button) ? 1 : 0);
			}
		}

		public MovieActionPaint(int startFrame, int endFrame, string button, float newS, TasMovie movie)
		{
			_newState = newS;
			FirstFrame = startFrame;
			LastFrame = endFrame;
			_buttonName = button;
			_isFloat = true;
			_oldState = new List<float>(endFrame - startFrame + 1);

			for (int i = 0; i < endFrame - startFrame + 1; i++)
			{
				_oldState.Add(movie.BoolIsPressed(startFrame + i, button) ? 1 : 0);
			}
		}

		public void Undo(TasMovie movie)
		{
			bool wasRecording = movie.ChangeLog.IsRecording;
			movie.ChangeLog.IsRecording = false;

			if (_isFloat)
			{
				for (int i = 0; i < _oldState.Count; i++)
				{
					movie.SetFloatState(FirstFrame + i, _buttonName, _oldState[i]);
				}
			}
			else
			{
				for (int i = 0; i < _oldState.Count; i++)
				{
					movie.SetBoolState(FirstFrame + i, _buttonName, _oldState[i] == 1);
				}
			}

			movie.ChangeLog.IsRecording = wasRecording;
		}

		public void Redo(TasMovie movie)
		{
			bool wasRecording = movie.ChangeLog.IsRecording;
			movie.ChangeLog.IsRecording = false;

			if (_isFloat)
			{
				movie.SetFloatStates(FirstFrame, LastFrame - FirstFrame + 1, _buttonName, _newState);
			}
			else
			{
				movie.SetBoolStates(FirstFrame, LastFrame - FirstFrame + 1, _buttonName, _newState == 1);
			}

			movie.ChangeLog.IsRecording = wasRecording;
		}
	}

	public class MovieActionBindInput : IMovieAction
	{
		public int FirstFrame { get; }
		public int LastFrame { get; }

		private readonly string _log;
		private readonly bool _delete;

		private readonly bool _bindMarkers;

		public MovieActionBindInput(TasMovie movie, int frame, bool isDelete)
		{
			FirstFrame = LastFrame = frame;
			_log = movie.GetInputLogEntry(frame);
			_delete = isDelete;
			_bindMarkers = movie.BindMarkersToInput;
		}

		public void Undo(TasMovie movie)
		{
			bool wasRecording = movie.ChangeLog.IsRecording;
			bool wasBinding = movie.BindMarkersToInput;
			movie.ChangeLog.IsRecording = false;
			movie.BindMarkersToInput = _bindMarkers;

			if (_delete) // Insert
			{
				movie.InsertInput(FirstFrame, _log);
				movie.InsertLagHistory(FirstFrame + 1, true);
			}
			else // Delete
			{
				movie.RemoveFrame(FirstFrame);
				movie.RemoveLagHistory(FirstFrame + 1);
			}

			movie.ChangeLog.IsRecording = wasRecording;
			movie.BindMarkersToInput = _bindMarkers;
		}

		public void Redo(TasMovie movie)
		{
			bool wasRecording = movie.ChangeLog.IsRecording;
			bool wasBinding = movie.BindMarkersToInput;
			movie.ChangeLog.IsRecording = false;
			movie.BindMarkersToInput = _bindMarkers;

			if (_delete)
			{
				movie.RemoveFrame(FirstFrame);
				movie.RemoveLagHistory(FirstFrame + 1);
			}
			else
			{
				movie.InsertInput(FirstFrame, _log);
				movie.InsertLagHistory(FirstFrame + 1, true);
			}

			movie.ChangeLog.IsRecording = wasRecording;
			movie.BindMarkersToInput = _bindMarkers;
		}
	}

	#endregion
}