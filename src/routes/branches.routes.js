import {Router} from 'express';
import { getBranchesById} from '../controllers/branches.controller.js'
const router= Router();

router.get("/branches/:id", getBranchesById);




export default router;