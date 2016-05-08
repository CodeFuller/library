using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CF.Core.Exceptions;

namespace CF.Scrum.Objects
{
	public abstract class BaseObject : IEquatable<BaseObject>
	{
		public string Id { get; set; }

		protected BaseObject(string id)
		{
			Id = id;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as BaseObject);
		}

		public bool Equals(BaseObject cmp)
		{
			return cmp != null && Id.Equals(cmp.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
