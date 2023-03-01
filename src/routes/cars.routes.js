import {Router} from 'express';
import {getCarsById} from '../controllers/cars.controller.js'

const router= Router();

router.get("/cars/:id", getCarsById);

export default router;