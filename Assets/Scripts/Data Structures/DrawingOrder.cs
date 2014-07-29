


public enum DrawingOrder {

	NONE = -1,
	UNDER_GROUND = 0, 
	GROUND,
	ON_GROUND, // Switches, grass, things right on top of the ground layer
	OBJECTS, // Gates, walls, characters, 
	ABOVE_OBJECTS,
	NUM_DRAWING_ORDERS

}
