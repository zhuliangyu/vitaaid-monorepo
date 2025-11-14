using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace MyToolkit.Base.Extensions
{
		public static class ReactiveUIExtension
		{
			public static IObservable<bool> ExecuteIfPossible<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd, TParam param) =>
				cmd.CanExecute.FirstAsync().Where(can => can).Do(async _ => await cmd.Execute(param));
			public static IObservable<bool> ExecuteIfPossible<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd) =>
				cmd.CanExecute.FirstAsync().Where(can => can).Do(async _ => await cmd.Execute());

			public static bool GetCanExecute<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd) =>
				cmd.CanExecute.FirstAsync().Wait();
		}
}
