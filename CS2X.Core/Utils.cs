﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CS2X.Core
{
	static class Utils
	{
		public static void DisposeInstances<T>(ref IEnumerable<T> instances) where T : class, IDisposable
		{
			if (instances != null)
			{
				foreach (var instance in instances)
				{
					instance.Dispose();
				}

				instances = null;
			}
		}

		public static void DisposeInstance<T>(ref T instance) where T : class, IDisposable
		{
			if (instance != null)
			{
				instance.Dispose();
				instance = null;
			}
		}

		public static bool Contains(this StringBuilder _this, char value)
		{
			for (int i = 0; i != _this.Length; ++i)
			{
				if (_this[i] == value) return true;
			}

			return false;
		}

		public static string GetAssemblyInfoVersion()
		{
			return Assembly.GetCallingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
		}

		public static string ReplaceNonWordChars(string value, char replacmentChar)
		{
			var newValue = new StringBuilder();
			foreach (char c in value)
			{
				if (!char.IsLetterOrDigit(c)) newValue.Append(replacmentChar);
				else newValue.Append(c);
			}

			return newValue.ToString();
		}
	}

	class Enabler : IDisposable
	{
		public bool enabled { get; private set; }

		public Enabler(bool enabled)
		{
			this.enabled = enabled;
		}

		public Enabler Enable()
		{
			enabled = true;
			return this;
		}

		public Enabler Disable()
		{
			enabled = false;
			return this;
		}

		public void Dispose()
		{
			enabled = !enabled;
		}
	}

	static class ISymbolExt
	{
		public static string FullName(this ISymbol symbol)
		{
			var fullName = new StringBuilder(symbol.Name);
			while (symbol != null)
			{
				if (symbol.ContainingType != null)
				{
					symbol = symbol.ContainingType;
					fullName.Insert(0, symbol.Name + '.');
				}
				else if (symbol.ContainingNamespace != null)
				{
					if (symbol.ContainingNamespace.IsGlobalNamespace) break;
					symbol = symbol.ContainingNamespace;
					fullName.Insert(0, symbol.Name + '.');
				}
				else
				{
					symbol = null;
				}
			}
			return fullName.ToString();
		}
	}
}
