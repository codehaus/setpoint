using System;
namespace setPoint.semantics{
	/// <summary>
	/// A dictionary with keys of type string and values of type PointCut
	/// </summary>
	[Serializable]
	public class PointCutMap: System.Collections.DictionaryBase {
		/// <summary>
		/// Initializes a new empty instance of the PointCutMap class
		/// </summary>
		public PointCutMap() {
			// empty
		}

		/// <summary>
		/// Gets or sets the PointCut associated with the given string
		/// </summary>
		/// <param name="key">
		/// The string whose value to get or set.
		/// </param>
		public virtual PointCut this[string key] {
			get {
				return (PointCut) this.Dictionary[key];
			}
			set {
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this PointCutMap.
		/// </summary>
		/// <param name="key">
		/// The string key of the element to add.
		/// </param>
		/// <param name="value">
		/// The PointCut value of the element to add.
		/// </param>
		public virtual void Add(string key, PointCut value) {
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this PointCutMap contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The string key to locate in this PointCutMap.
		/// </param>
		/// <returns>
		/// true if this PointCutMap contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(string key) {
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this PointCutMap contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The string key to locate in this PointCutMap.
		/// </param>
		/// <returns>
		/// true if this PointCutMap contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(string key) {
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this PointCutMap contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The PointCut value to locate in this PointCutMap.
		/// </param>
		/// <returns>
		/// true if this PointCutMap contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(PointCut value) {
			foreach (PointCut item in this.Dictionary.Values) {
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this PointCutMap.
		/// </summary>
		/// <param name="key">
		/// The string key of the element to remove.
		/// </param>
		public virtual void Remove(string key) {
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this PointCutMap.
		/// </summary>
		public virtual System.Collections.ICollection Keys {
			get {
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this PointCutMap.
		/// </summary>
		public virtual System.Collections.ICollection Values {
			get {
				return this.Dictionary.Values;
			}
		}
	}
}