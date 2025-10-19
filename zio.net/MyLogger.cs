using Microsoft.Extensions.Logging;

namespace zio.net
{
	public sealed class MyLogger
	{
		private static Lazy<MyLogger> _instance;

		private ILoggerFactory _loggerFactory;

		private MyLogger() {
			_loggerFactory = LoggerFactory.Create(builder => {
				builder.AddFilter((category, level) => level >= LogLevel.Debug).AddConsole();
			});
		}

		private MyLogger(ILoggerFactory loggerFactory) {
			_loggerFactory = loggerFactory;
		}

		public static void Config() {
			_instance = new Lazy<MyLogger>(() => new MyLogger());
		}

		public static void Config(ILoggerFactory loggerFactory) {
			_instance = new Lazy<MyLogger>(() => new MyLogger(loggerFactory));
		}

		public static ILogger GetLogger(String name) {
			return _instance.Value._loggerFactory.CreateLogger(name);
		}

		public static void AddProvider(ILoggerProvider provider) {
			_instance.Value._loggerFactory.AddProvider(provider);
		}
	}
}
