﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BizHawk.Emulation.Common
{
	/// <summary>
	/// Defines the schema for all the currently available controls for an IEmulator instance
	/// </summary>
	/// <seealso cref="IEmulator" /> 
	public class ControllerDefinition
	{
		public ControllerDefinition()
		{
			BoolButtons = new List<string>();
			FloatControls = new List<string>();
			FloatRanges = new List<FloatRange>();
			AxisConstraints = new List<AxisConstraint>();
			CategoryLabels = new Dictionary<string, string>();
		}

		public ControllerDefinition(ControllerDefinition source)
			: this()
		{
			Name = source.Name;
			BoolButtons.AddRange(source.BoolButtons);
			FloatControls.AddRange(source.FloatControls);
			FloatRanges.AddRange(source.FloatRanges);
			AxisConstraints.AddRange(source.AxisConstraints);
			CategoryLabels = source.CategoryLabels;
		}

		/// <summary>
		/// Gets or sets the name of the controller definition
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a list of all button types that have a boolean (on/off) value
		/// </summary>
		public List<string> BoolButtons { get; set; }

		/// <summary>
		/// Gets a list of all non-boolean types, that can be represented by a numerical value (such as analog controls, stylus coordinates, etc
		/// </summary>
		public List<string> FloatControls { get; }

		/// <summary>
		/// Gets a list of all float ranges for each float control (must be one to one with FloatControls)
		/// FloatRanges include the min/max/default values
		/// </summary>
		public List<FloatRange> FloatRanges { get; }

		/// <summary>
		/// Gets the axis constraints that apply artificial constraints to float values
		/// For instance, a N64 controller's analog range is actually larger than the amount allowed by the plastic that artificially constrains it to lower values
		/// Axis constraints provide a way to technically allow the full range but have a user option to constrain down to typical values that a real control would have
		/// </summary>
		public List<AxisConstraint> AxisConstraints { get; }

		/// <summary>
		/// Gets the category labels. These labels provide a means of categorizing controls in various controller display and config screens
		/// </summary>
		public Dictionary<string, string> CategoryLabels { get; }

		public void ApplyAxisConstraints(string constraintClass, IDictionary<string, float> floatButtons)
		{
			if (AxisConstraints == null)
			{
				return;
			}

			foreach (var constraint in AxisConstraints)
			{
				if (constraint.Class != constraintClass)
				{
					continue;
				}

				switch (constraint.Type)
				{
					case AxisConstraintType.Circular:
						{
							string xaxis = constraint.Params[0] as string;
							string yaxis = constraint.Params[1] as string;
							float range = (float)constraint.Params[2];
							if (!floatButtons.ContainsKey(xaxis)) break;
							if (!floatButtons.ContainsKey(yaxis)) break;
							double xval = floatButtons[xaxis];
							double yval = floatButtons[yaxis];
							double length = Math.Sqrt((xval * xval) + (yval * yval));
							if (length > range)
							{
								double ratio = range / length;
								xval *= ratio;
								yval *= ratio;
							}

							floatButtons[xaxis] = (float)xval;
							floatButtons[yaxis] = (float)yval;
							break;
						}
				}
			}
		}

		public struct FloatRange
		{
			public readonly float Min;
			public readonly float Max;

			/// <summary>
			/// default position
			/// </summary>
			public readonly float Mid;

			public FloatRange(float min, float mid, float max)
			{
				Min = min;
				Mid = mid;
				Max = max;
			}

			// for terse construction
			public static implicit operator FloatRange(float[] f)
			{
				if (f.Length != 3)
				{
					throw new ArgumentException();
				}

				return new FloatRange(f[0], f[1], f[2]);
			}

			/// <summary>
			/// Gets maximum decimal digits analog input can occupy. Discards negative sign and possible fractional part (analog devices don't use floats anyway).
			/// </summary>
			public int MaxDigits()
			{
				return Math.Max(
					Math.Abs((int)Min).ToString().Length,
					Math.Abs((int)Max).ToString().Length);
			}
		}

		public enum AxisConstraintType
		{
			Circular
		}

		public struct AxisConstraint
		{
			public string Class;
			public AxisConstraintType Type;
			public object[] Params;
		}

		/// <summary>
		/// Gets a list of controls put in a logical order such as by controller number,
		/// This is a default implementation that should work most of the time
		/// </summary>
		public virtual IEnumerable<IEnumerable<string>> ControlsOrdered
		{
			get
			{
				List<string> list = new List<string>(FloatControls);
				list.AddRange(BoolButtons);

				// starts with console buttons, then each plasyer's buttons individually
				List<string>[] ret = new List<string>[PlayerCount + 1];
				for (int i = 0; i < ret.Length; i++)
				{
					ret[i] = new List<string>();
				}

				foreach (string btn in list)
				{
					ret[PlayerNumber(btn)].Add(btn);
				}

				return ret;
			}
		}

		public int PlayerNumber(string buttonName)
		{
			var match = PlayerRegex.Match(buttonName);
			if (match.Success)
			{
				return int.Parse(match.Groups[1].Value);
			}
			else
			{
				return 0;
			}
		}

		private static readonly Regex PlayerRegex = new Regex("^P(\\d+) ");

		public int PlayerCount
		{
			get
			{
				var allNames = FloatControls.Concat(BoolButtons).ToList();
				var player = allNames
					.Select(PlayerNumber)
					.DefaultIfEmpty(0)
					.Max();

				if (player > 0)
				{
					return player;
				}

				// Hack for things like gameboy/ti-83 as opposed to genesis with no controllers plugged in
				if (allNames.Any(b => b.StartsWith("Up")))
				{
					return 1;
				} 

				return 0;
			}
		}

		public bool Any()
		{
			return BoolButtons.Any() || FloatControls.Any();
		}
	}
}
