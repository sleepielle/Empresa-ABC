
import {employees}  from '../db.js';


export const getEmployeesByUsername = async (req, res)=> {
   
    const [result] = await employees.query(`SELECT first_name, last_name, ID FROM employees WHERE username = ?`, [req.params.username] );
     res.json(result);
 };


