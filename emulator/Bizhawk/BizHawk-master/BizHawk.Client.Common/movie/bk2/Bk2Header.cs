﻿using System.Collections.Generic;
using System.Text;

namespace BizHawk.Client.Common
{
	public class Bk2Header : Dictionary<string, string>
	{
		public new string this[string key]
		{
			get
			{
				return ContainsKey(key) ? base[key] : "";
			}

			set
			{
				if (ContainsKey(key))
				{
					base[key] = value;
				}
				else
				{
					Add(key, value);
				}
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			foreach (var kvp in this)
			{
				sb
					.Append(kvp.Key)
					.Append(' ')
					.Append(kvp.Value)
					.AppendLine();
			}

			return sb.ToString();
		}
	}
}
