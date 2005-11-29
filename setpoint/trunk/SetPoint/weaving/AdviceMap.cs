using setPoint.semantics;

namespace setPoint.weaving{
	/// <summary>
	/// A dictionary with keys of type PointCut and values of type TriggerSet
	/// </summary>
	public class AdviceMap: System.Collections.DictionaryBase {
		/// <summary>
		/// Initializes a new empty instance of the AdviceMap class
		/// </summary>
		public AdviceMap() {
			// empty
		}

		/// <summary>
		/// Gets or sets the TriggerSet associated with the given PointCut
		/// </summary>
		/// <param name="key">
		/// The PointCut whose value to get or set.
		/// </param>
		public virtual TriggerSet this[PointCut key] {
			get {
				return (TriggerSet) this.Dictionary[key];
			}
			set {
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this AdviceMap.
		/// </summary>
		/// <param name="key">
		/// The PointCut key of the element to add.
		/// </param>
		/// <param name="value">
		/// The TriggerSet value of the element to add.
		/// </param>
		public virtual void Add(PointCut key, TriggerSet value) {
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this AdviceMap contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The PointCut key to locate in this AdviceMap.
		/// </param>
		/// <returns>
		/// true if this AdviceMap contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(PointCut key) {
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this AdviceMap contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The PointCut key to locate in this AdviceMap.
		/// </param>
		/// <returns>
		/// true if this AdviceMap contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(PointCut key) {
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this AdviceMap contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The TriggerSet value to locate in this AdviceMap.
		/// </param>
		/// <returns>
		/// true if this AdviceMap contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(TriggerSet value) {
			foreach (TriggerSet item in this.Dictionary.Values) {
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this AdviceMap.
		/// </summary>
		/// <param name="key">
		/// The PointCut key of the element to remove.
		/// </param>
		public virtual void Remove(PointCut key) {
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this AdviceMap.
		/// </summary>
		public virtual System.Collections.ICollection Keys {
			get {
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this AdviceMap.
		/// </summary>
		public virtual System.Collections.ICollection Values {
			get {
				return this.Dictionary.Values;
			}
		}
	}

}