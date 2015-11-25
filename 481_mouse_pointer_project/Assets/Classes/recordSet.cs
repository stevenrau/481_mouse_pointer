using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class recordSet {

//	private List<record> set;
//	
//	public recordSet () {
//		List<record> set = new List<record> ();
//	}
//
//	public void addRecord (float t, int m, int b) {
//		record newRecord = new record (t,m,b);
//		set.Add (newRecord);
//	}
//
//	public List<record> getList() {
//		return set;
//	}


	private record[] set;
	private int index = 0;
	private int length;

	public recordSet(int l) {
		length = l;
		set = new record[length];
	}

	public void addRecord (float t, string mn, string bn, int n, bool skip) {
		record newRecord = new record (t,mn,bn,n,skip);
		set [index] = newRecord;
		index ++;
	}

	public record[] getArray() {
		return set;
	}

	public record getItem(int i) {
		return set [i];
	}

}
