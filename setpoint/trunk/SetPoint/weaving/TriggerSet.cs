namespace setPoint.weaving{
	/// <summary>
	/// A collection of elements of type ITrigger
	/// </summary>
	public class TriggerSet: System.Collections.CollectionBase {
		/// <summary>
		/// Initializes a new empty instance of the TriggerSet class.
		/// </summary>
		public TriggerSet() {
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the TriggerSet class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new TriggerSet.
		/// </param>
		public TriggerSet(Trigger[] items) {
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the TriggerSet class, containing elements
		/// copied from another instance of TriggerSet
		/// </summary>
		/// <param name="items">
		/// The TriggerSet whose elements are to be added to the new TriggerSet.
		/// </param>
		public TriggerSet(TriggerSet items) {
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this TriggerSet.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this TriggerSet.
		/// </param>
		public virtual void AddRange(Trigger[] items) {
			foreach (Trigger item in items) {
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another TriggerSet to the end of this TriggerSet.
		/// </summary>
		/// <param name="items">
		/// The TriggerSet whose elements are to be added to the end of this TriggerSet.
		/// </param>
		public virtual void AddRange(TriggerSet items) {
			foreach (Trigger item in items) {
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type ITrigger to the end of this TriggerSet.
		/// </summary>
		/// <param name="value">
		/// The ITrigger to be added to the end of this TriggerSet.
		/// </param>
		public virtual void Add(Trigger value) {
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic ITrigger value is in this TriggerSet.
		/// </summary>
		/// <param name="value">
		/// The ITrigger value to locate in this TriggerSet.
		/// </param>
		/// <returns>
		/// true if value is found in this TriggerSet;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(Trigger value) {
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this TriggerSet
		/// </summary>
		/// <param name="value">
		/// The ITrigger value to locate in the TriggerSet.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(Trigger value) {
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the TriggerSet at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the ITrigger is to be inserted.
		/// </param>
		/// <param name="value">
		/// The ITrigger to insert.
		/// </param>
		public virtual void Insert(int index, Trigger value) {
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the ITrigger at the given index in this TriggerSet.
		/// </summary>
		public virtual Trigger this[int index] {
			get {
				return (Trigger) this.List[index];
			}
			set {
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific ITrigger from this TriggerSet.
		/// </summary>
		/// <param name="value">
		/// The ITrigger value to remove from this TriggerSet.
		/// </param>
		public virtual void Remove(Trigger value) {
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by TriggerSet.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator {
			private System.Collections.IEnumerator wrapped;

			public Enumerator(TriggerSet collection) {
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public Trigger Current {
				get {
					return (Trigger) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current {
				get {
					return (Trigger) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this TriggerSet.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual TriggerSet.Enumerator GetEnumerator() {
			return new TriggerSet.Enumerator(this);
		}
	}
}