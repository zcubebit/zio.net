using Microsoft.Extensions.Logging;

namespace zio.net
{
	public sealed class MyLogger
	{
		private static Lazy<MyLogger> _INSTANCE;

		private ILoggerFactory loggerFactory;

		private MyLogger() {
			loggerFactory = LoggerFactory.Create(builder => {
				builder.AddFilter((category, level) => level >= LogLevel.Debug).AddConsole();
			});
		}

		private MyLogger(ILoggerFactory loggerFactory) {
			this.loggerFactory = loggerFactory;
		}

		public static void Config(ILoggerFactory loggerFactory) {
			_INSTANCE = new Lazy<MyLogger>(() => new MyLogger());
		}

		public static ILogger GetLogger(String name) {
			return _INSTANCE.Value.loggerFactory.CreateLogger(name);
		}

		public static void AddProvider(ILoggerProvider provider) {
			_INSTANCE.Value.loggerFactory.AddProvider(provider);
		}
	}
}
