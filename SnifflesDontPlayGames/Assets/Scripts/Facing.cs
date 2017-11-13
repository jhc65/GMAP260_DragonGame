/*
 * "Enum" class for direction string 
 * up down left right
 */
public class FacingDir {
	private string dir;

	/*
	 * State int for animation controller
	 * Left = 0
	 * Right = 1
	 * Up = 2
	 * Down = 3
	 */
	private int state; 

	// Default constructor
	public FacingDir() {
		dir = "left";
		state = 0;
	}

	public FacingDir(string direction) {
		SetDir(direction);
	}

	public void SetDir(string direction) {
		dir = direction;
		if (IsLeft()) 
			state = 0;
		if (IsRight())
			state = 1;
		if (IsUp())
			state = 2;
		if (IsDown())
			state =3;
	}

	public string GetDir() {
		return dir;
	}

	public int GetInt() {
		return state;
	}

	public bool Equals(FacingDir d) {
		return dir.Equals(d.GetDir());
	}

	public bool IsUp() {
		return dir.Equals("up");
	}

	public bool IsLeft() {
		return dir.Equals("left");
	}

	public bool IsDown() {
		return dir.Equals("down");
	}

	public bool IsRight() {
		return dir.Equals("right");
	}

	public void Flip() {
		if (IsRight())
			SetDir("left");
		else if (IsLeft())
			SetDir("right");
		else if (IsUp())
			SetDir("down");
		else
			SetDir("up");
	}
}

public static class Dirs {
	public static FacingDir left = new FacingDir("left");
	public static FacingDir right = new FacingDir("right");
	public static FacingDir down = new FacingDir("down");
	public static FacingDir up = new FacingDir("up");
}

