using SQLite;
using System.Collections.Generic;

public class LevelInfo {
	[PrimaryKey]
	public int LevelNum {get; set;}
	public float Time {get; set;}
	public int Medal {get; set;}
}
