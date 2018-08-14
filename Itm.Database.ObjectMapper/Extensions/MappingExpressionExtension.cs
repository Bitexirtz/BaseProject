﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Reflection;

namespace Itm.Database.ObjectMapper.Extensions
{
	internal static class MappingExpressionExtension
	{
		public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
		{
			var flags = BindingFlags.Public | BindingFlags.Instance;
			var sourceType = typeof (TSource);
			var destinationProperties = typeof (TDestination).GetProperties (flags);

			foreach (var property in destinationProperties) {
				if (sourceType.GetProperty (property.Name, flags) == null) {
					expression.ForMember (property.Name, opt => opt.Ignore ());
				}
			}
			return expression;
		}
	}
}
