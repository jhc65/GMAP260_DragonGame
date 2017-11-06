/*
 * "Enum" class for direction string 
 * up down left right
 */
public class FacingDir {
	private string dir;

	// Default constructor
	public FacingDir() {
		dir = "left";
	}

	public FacingDir(string direction) {
		dir = direction;
	}

	public void SetDir(string direction) {
		dir = direction;
	}

	public string GetDir() {
		return dir;
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
			dir = "left";
		else if (IsLeft())
			dir = "right";
		else if (IsUp())
			dir = "down";
		else
			dir = "up";
	}
}

public static class Dirs {
	public static FacingDir left = new FacingDir("left");
	public static FacingDir right = new FacingDir("right");
	public static FacingDir down = new FacingDir("down");
	public static FacingDir up = new FacingDir("up");
}

