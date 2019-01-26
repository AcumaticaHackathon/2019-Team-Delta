﻿using System;
using System.Collections.Generic;
using System.Text;

using PX.Data;
using PX.Data.SQLTree;

namespace PX.Objects.AP.BQL
{
	/// <summary>
	/// A BQL predicate checking whether its operand (returning a 
	/// screen ID) corresponds to one of the Expense Claims screens.
	/// </summary>
	[Obsolete("This predicate is not used anymore and will be removed in Acumatica ERP 8.0. Use APRegister.OrigModule == 'EP' comparison instead.", false)]
	public class IsFromExpenseClaims<TScreenID> : IBqlUnary
		where TScreenID : IBqlOperand
	{
		public class screenExpenseClaims : Constant<string>
		{
			public screenExpenseClaims() : base("EP301030")
			{ }
		}
		public class screenExpenseClaim : Constant<string>
		{
			public screenExpenseClaim() : base("EP301000")
			{ }
		}
		public class screenReleaseExpenseClaims : Constant<string>
		{
			public screenReleaseExpenseClaims() : base("EP501000")
			{ }
		}

		private IBqlUnary _where = new Where<
			TScreenID, Equal<screenExpenseClaim>,
			Or<TScreenID, Equal<screenExpenseClaims>,
			Or<TScreenID, Equal<screenReleaseExpenseClaims>>>>();

		public bool AppendExpression(ref SQLExpression exp, PXGraph graph, BqlCommandInfo info,
			BqlCommand.Selection selection)
			=> _where.AppendExpression(ref exp, graph, info, selection);

		public void Parse(PXGraph graph, List<IBqlParameter> pars, List<Type> tables, List<Type> fields, List<IBqlSortColumn> sortColumns, StringBuilder text, BqlCommand.Selection selection)
			=> _where.Parse(graph, pars, tables, fields, sortColumns, text, selection);

		public void Verify(PXCache cache, object item, List<object> pars, ref bool? result, ref object value)
			=> _where.Verify(cache, item, pars, ref result, ref value);
	}
}
