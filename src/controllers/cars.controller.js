import {cars} from '../db.js';

export const getCarsById = async (req, res) =>{
    const id = req.params.id;
    const [gCars] = await cars.query("SELECT make,model,year FROM cars WHERE id = ?", [id]);
    res.json(gCars);
}