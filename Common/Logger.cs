﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
	public enum LogLevel
	{
		Debug,
		Info,
		Warning,
		Error,
		Critical
	}

	public class Logger
	{
		public event Action OnMessageLogged;

		private List<ILoggerTarget> targets;



		public Logger()
		{
			targets = new List<ILoggerTarget>();
		}

		public void AddTarget(ILoggerTarget target)
		{
			targets.Add(target);
		}

		public void Log(string message, LogLevel logLevel)
		{
			foreach (ILoggerTarget t in targets)
				t.Log(message, logLevel);

			if (OnMessageLogged != null)
				OnMessageLogged();
		}
	}

	public interface ILoggerTarget
	{
		void Log(string message, LogLevel logLevel);
	}

	public class LoggerConsoleTarget : ILoggerTarget
	{
		public void Log(string message, LogLevel logLevel)
		{
			// Set console color according to log level
			switch (logLevel)
			{
				case LogLevel.Debug:
					Console.ForegroundColor = ConsoleColor.White;
					Console.BackgroundColor = ConsoleColor.DarkGreen;
					break;
				case LogLevel.Info:
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					break;
				case LogLevel.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case LogLevel.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case LogLevel.Critical:
					Console.ForegroundColor = ConsoleColor.White;
					Console.BackgroundColor = ConsoleColor.Red;
					break;
			}

			// Print the message
			Console.WriteLine($"{DateTime.Now} [{logLevel.ToString().ToUpper()}]: {message}");

			// Reset console colors
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.BackgroundColor = ConsoleColor.Black;
		}
	}

	public class LoggerFileTarget : ILoggerTarget
	{
		private string path;



		public LoggerFileTarget(string filename)
		{
			path = filename;
		}

		public void Log(string message, LogLevel logLevel)
		{
			string text = $"{DateTime.Now} [{logLevel.ToString().ToUpper()}]: {message}" + Environment.NewLine;

			File.AppendAllText(path, text);
		}
	}
}
