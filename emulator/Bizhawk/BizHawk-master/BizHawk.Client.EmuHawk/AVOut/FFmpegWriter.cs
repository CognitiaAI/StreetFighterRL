﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using BizHawk.Client.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	/// <summary>
	/// uses pipes to launch an external ffmpeg process and encode
	/// </summary>
	[VideoWriter("ffmpeg", "FFmpeg writer", "Uses an external FFMPEG process to encode video and audio.  Various formats supported.  Splits on resolution change.")]
	public class FFmpegWriter : IVideoWriter
	{
		/// <summary>
		/// handle to external ffmpeg process
		/// </summary>
		private Process _ffmpeg;

		/// <summary>
		/// the commandline actually sent to ffmpeg; for informative purposes
		/// </summary>
		private string _commandline;

		/// <summary>
		/// current file segment (for multires)
		/// </summary>
		private int _segment;

		/// <summary>
		/// base filename before segment number is attached
		/// </summary>
		private string _baseName;

		/// <summary>
		/// recent lines in ffmpeg's stderr, for informative purposes
		/// </summary>
		private Queue<string> _stderr;

		/// <summary>
		/// number of lines of stderr to buffer
		/// </summary>
		private const int Consolebuffer = 5;

		/// <summary>
		/// muxer handle for the current segment
		/// </summary>
		private NutMuxer _muxer;

		/// <summary>
		/// codec token in use
		/// </summary>
		private FFmpegWriterForm.FormatPreset _token;

		/// <summary>
		/// file extension actually used
		/// </summary>
		private string _ext;

		public void SetFrame(int frame)
		{
		}

		public void OpenFile(string baseName)
		{
			_baseName = Path.Combine(
				Path.GetDirectoryName(baseName),
				Path.GetFileNameWithoutExtension(baseName));

			_ext = Path.GetExtension(baseName);

			_segment = 0;
			OpenFileSegment();
		}
		
		/// <summary>
		/// starts an ffmpeg process and sets up associated sockets
		/// </summary>
		private void OpenFileSegment()
		{
			try
			{
				_ffmpeg = new Process();
#if WINDOWS
				_ffmpeg.StartInfo.FileName = Path.Combine(PathManager.GetDllDirectory(), "ffmpeg.exe");
#else
				ffmpeg.StartInfo.FileName = "ffmpeg"; // expecting native version to be in path
#endif

				string filename = $"{_baseName}_{_segment,4:D4}{_ext}";
				_ffmpeg.StartInfo.Arguments = string.Format("-y -f nut -i - {1} \"{0}\"", filename, _token.Commandline);
				_ffmpeg.StartInfo.CreateNoWindow = true;

				// ffmpeg sends informative display to stderr, and nothing to stdout
				_ffmpeg.StartInfo.RedirectStandardError = true;
				_ffmpeg.StartInfo.RedirectStandardInput = true;
				_ffmpeg.StartInfo.UseShellExecute = false;

				_commandline = "ffmpeg " + _ffmpeg.StartInfo.Arguments;

				_ffmpeg.ErrorDataReceived += new DataReceivedEventHandler(StderrHandler);

				_stderr = new Queue<string>(Consolebuffer);

				_ffmpeg.Start();
			}
			catch
			{
				_ffmpeg.Dispose();
				_ffmpeg = null;
				throw;
			}

			_ffmpeg.BeginErrorReadLine();

			_muxer = new NutMuxer(width, height, fpsnum, fpsden, sampleRate, channels, _ffmpeg.StandardInput.BaseStream);
		}

		/// <summary>
		/// saves stderr lines from ffmpeg in a short queue
		/// </summary>
		private void StderrHandler(object p, DataReceivedEventArgs line)
		{
			if (!string.IsNullOrEmpty(line.Data))
			{
				if (_stderr.Count == Consolebuffer)
				{
					_stderr.Dequeue();
				}

				_stderr.Enqueue(line.Data + "\n");
			}
		}

		/// <summary>
		/// finishes an ffmpeg process
		/// </summary>
		private void CloseFileSegment()
		{
			_muxer.Finish();
			//ffmpeg.StandardInput.Close();

			// how long should we wait here?
			_ffmpeg.WaitForExit(20000);
			_ffmpeg.Dispose();
			_ffmpeg = null;
			_stderr = null;
			_commandline = null;
			_muxer = null;
		}


		public void CloseFile()
		{
			CloseFileSegment();
			_baseName = null;
		}

		/// <summary>
		/// returns a string containing the commandline sent to ffmpeg and recent console (stderr) output
		/// </summary>
		/// <returns></returns>
		private string ffmpeg_geterror()
		{
			if (_ffmpeg.StartInfo.RedirectStandardError)
			{
				_ffmpeg.CancelErrorRead();
			}

			var s = new StringBuilder();
			s.Append(_commandline);
			s.Append('\n');
			while (_stderr.Count > 0)
			{
				var foo = _stderr.Dequeue();
				s.Append(foo);
			}

			return s.ToString();
		}


		public void AddFrame(IVideoProvider source)
		{
			if (source.BufferWidth != width || source.BufferHeight != height)
			{
				SetVideoParameters(source.BufferWidth, source.BufferHeight);
			}

			if (_ffmpeg.HasExited)
			{
				throw new Exception("unexpected ffmpeg death:\n" + ffmpeg_geterror());
			}

			var video = source.GetVideoBuffer();
			try
			{
				_muxer.WriteVideoFrame(video);
			}
			catch
			{
				MessageBox.Show("Exception! ffmpeg history:\n" + ffmpeg_geterror());
				throw;
			}

			// have to do binary write!
			//ffmpeg.StandardInput.BaseStream.Write(b, 0, b.Length);
		}

		public IDisposable AcquireVideoCodecToken(IWin32Window hwnd)
		{
			return FFmpegWriterForm.DoFFmpegWriterDlg(hwnd);
		}
	
		public void SetVideoCodecToken(IDisposable token)
		{
			if (token is FFmpegWriterForm.FormatPreset)
			{
				_token = (FFmpegWriterForm.FormatPreset)token;
			}
			else
			{
				throw new ArgumentException("FFmpegWriter can only take its own codec tokens!");
			}
		}

		/// <summary>
		/// video params
		/// </summary>
		private int fpsnum, fpsden, width, height, sampleRate, channels;

		public void SetMovieParameters(int fpsnum, int fpsden)
		{
			this.fpsnum = fpsnum;
			this.fpsden = fpsden;
		}

		public void SetVideoParameters(int width, int height)
		{
			this.width = width;
			this.height = height;

			/* ffmpeg theoretically supports variable resolution videos, but in practice that's not handled very well.
			 * so we start a new segment.
			 */
			if (_ffmpeg != null)
			{
				CloseFileSegment();
				_segment++;
				OpenFileSegment();
			}
		}


		public void SetMetaData(string gameName, string authors, ulong lengthMS, ulong rerecords)
		{
			// can be implemented with ffmpeg "-metadata" parameter???
			// nyi
		}

		public void Dispose()
		{
			if (_ffmpeg != null)
			{
				CloseFile();
			}
		}

		public void AddSamples(short[] samples)
		{
			if (_ffmpeg.HasExited)
			{
				throw new Exception("unexpected ffmpeg death:\n" + ffmpeg_geterror());
			}

			if (samples.Length == 0)
			{
				// has special meaning for the muxer, so don't pass on
				return;
			}

			try
			{
				_muxer.WriteAudioFrame(samples);
			}
			catch
			{
				MessageBox.Show("Exception! ffmpeg history:\n" + ffmpeg_geterror());
				throw;
			}
		}

		public void SetAudioParameters(int sampleRate, int channels, int bits)
		{
			if (bits != 16)
			{
				throw new ArgumentOutOfRangeException(nameof(bits), "Sampling depth must be 16 bits!");
			}

			this.sampleRate = sampleRate;
			this.channels = channels;
		}

		public string DesiredExtension()
		{
			// this needs to interface with the codec token
			return _token.Defaultext;
		}

		public void SetDefaultVideoCodecToken()
		{
			_token = FFmpegWriterForm.FormatPreset.GetDefaultPreset();
		}

		public bool UsesAudio => true;

		public bool UsesVideo => true;
	}
}
