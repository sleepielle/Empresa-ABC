import {branches} from '../db.js';

export const getBranchesById = async (req, res) =>{
    const id = req.params.id;
    const [gSales] = await branches.query("SELECT country, state FROM branches WHERE id = ?", [id]);
    res.json(gSales);
}