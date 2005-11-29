namespace setPoint.semantics{
	/// <summary>
	/// A collection of elements of type PointCut
	/// </summary>
	public class PointCutSet: System.Collections.CollectionBase {
		/// <summary>
		/// Initializes a new empty instance of the PointCutSet class.
		/// </summary>
		public PointCutSet() {
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the PointCutSet class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new PointCutSet.
		/// </param>
		public PointCutSet(PointCut[] items) {
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the PointCutSet class, containing elements
		/// copied from another instance of PointCutSet
		/// </summary>
		/// <param name="items">
		/// The PointCutSet whose elements are to be added to the new PointCutSet.
		/// </param>
		public PointCutSet(PointCutSet items) {
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this PointCutSet.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this PointCutSet.
		/// </param>
		public virtual void AddRange(PointCut[] items) {
			foreach (PointCut item in items) {
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another PointCutSet to the end of this PointCutSet.
		/// </summary>
		/// <param name="items">
		/// The PointCutSet whose elements are to be added to the end of this PointCutSet.
		/// </param>
		public virtual void AddRange(PointCutSet items) {
			foreach (PointCut item in items) {
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type PointCut to the end of this PointCutSet.
		/// </summary>
		/// <param name="value">
		/// The PointCut to be added to the end of this PointCutSet.
		/// </param>
		public virtual void Add(PointCut value) {
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic PointCut value is in this PointCutSet.
		/// </summary>
		/// <param name="value">
		/// The PointCut value to locate in this PointCutSet.
		/// </param>
		/// <returns>
		/// true if value is found in this PointCutSet;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(PointCut value) {
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this PointCutSet
		/// </summary>
		/// <param name="value">
		/// The PointCut value to locate in the PointCutSet.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(PointCut value) {
			return this.List.IndexOf(value);
		}		

		/// <summary>
		/// Inserts an element into the PointCutSet at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the PointCut is to be inserted.
		/// </param>
		/// <param name="value">
		/// The PointCut to insert.
		/// </param>
		public virtual void Insert(int index, PointCut value) {
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the PointCut at the given index in this PointCutSet.
		/// </summary>
		public virtual PointCut this[int index] {
			get {
				return (PointCut) this.List[index];
			}
			set {
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific PointCut from this PointCutSet.
		/// </summary>
		/// <param name="value">
		/// The PointCut value to remove from this PointCutSet.
		/// </param>
		public virtual void Remove(PointCut value) {
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by PointCutSet.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator {
			private System.Collections.IEnumerator wrapped;

			public Enumerator(PointCutSet collection) {
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public PointCut Current {
				get {
					return (PointCut) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current {
				get {
					return (PointCut) (this.wrapped.Current);
				}
			}

			public bool MoveNext() {
				return this.wrapped.MoveNext();
			}

			public void Reset() {
				this.wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this PointCutSet.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual PointCutSet.Enumerator GetEnumerator() {
			return new PointCutSet.Enumerator(this);
		}
	}
}