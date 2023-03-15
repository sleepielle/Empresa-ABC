import {branches} from '../db.js';
import amqp from "amqplib";
let gSales = {};


async function sendToRabbitMQ( message) {

  try {
    // Connect to RabbitMQ server
    const connection = await amqp.connect("amqp://localhost:5672");

    // Create a channel
    const channel = await connection.createChannel();

    // Define exchange name and type
    const exchangeName = "MECHE";
    const exchangeType = "direct";

    // Create exchange if it doesn't exist
    await channel.assertExchange(exchangeName, exchangeType, {
      durable: false,
    });

    const queueName = "branchesQueue";
    await channel.assertQueue(queueName, {
      durable: true,
    });



    // Define message to send
   

    // Define routing key
    const routingKey = "myRoutingKey";

    // Publish message to exchange with confirm option
    const isSent = await channel.publish(
      exchangeName,
      routingKey,
      Buffer.from(message),
      { confirm: true }
    );


    channel.sendToQueue(queueName, Buffer.from(message), {
      persistent: true,
   });

    if (isSent) {
        console.log(`Sent message to ${queueName}: ${message}`);
    } else {
      console.log("Message could not be sent to RabbitMQ");
    }

    await channel.close();
    await connection.close();
  } catch (error) {
    console.error(error);
  }
    
}

export const getBranchesById = async (req, res) =>{
    const id = req.params.id;
    //se extrae el primer elemento de la respuesta. 
   [gSales]= await branches.query("SELECT id,country, state FROM branches WHERE id = ?", [id]);
    res.json(gSales);
var message=JSON.stringify(gSales[0], null, 4)
    console.table(gSales);
    await sendToRabbitMQ(message);
}




  export {gSales};