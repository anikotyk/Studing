import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;

public class Main {
    public static void main(String[] args) {
        try{
            Connection connection = DriverManager.getConnection("jdbc:mysql://localhost:3306/autosalon", "root", "13013a");
            Statement statement = connection.createStatement();

            System.out.println("Before changes:");
            ResultSet resultSet = statement.executeQuery("select * from carproducers");

            while (resultSet.next()){
                System.out.println(resultSet.getString("carProducersName"));
            }

            resultSet = statement.executeQuery("select * from marks");

            while (resultSet.next()){
                System.out.println(resultSet.getString("markName"));
            }

            statement.executeUpdate("UPDATE `autosalon`.`carproducers`\n" +
                    "            SET\n" +
                    "`id` = 0,\n" +
                    "`carProducersName` = 'producer1'\n" +
                    "            WHERE `id` = 0;");

            statement.executeUpdate("UPDATE `autosalon`.`carproducers`\n" +
                    "            SET\n" +
                    "`id` = 1,\n" +
                    "`carProducersName` = 'producer2'\n" +
                    "            WHERE `id` = 1;");


            //statement.executeUpdate("INSERT OR REPLACE INTO carproducers " + "VALUES (1, 'producer2')");
            System.out.println("\nAfter changes:");
            resultSet = statement.executeQuery("select * from carproducers");

            while (resultSet.next()){
                System.out.println(resultSet.getString("carProducersName"));
            }

            statement.executeUpdate("UPDATE `autosalon`.`marks`\n" +
                    "SET\n" +
                    "`id` = 0,\n" +
                    "`markName` = 'mark1',\n" +
                    "`idProducer` = 0\n" +
                    "WHERE `id` = 0;\n");

            resultSet = statement.executeQuery("select * from marks");

            while (resultSet.next()){
                System.out.println(resultSet.getString("markName"));
            }

            System.out.println("Get marks from producer 0");

            resultSet = statement.executeQuery("SELECT * FROM marks WHERE `idProducer` = 0");
            while (resultSet.next()){
                System.out.println(resultSet.getString("markName"));
            }

        }
        catch(Exception e){
           e.printStackTrace();
        }
    }
}