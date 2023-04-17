import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.Method;

public class Main {
    public static void main(String[] args) throws ClassNotFoundException {
        ClassLoaderCustom classLoaderCustom = new ClassLoaderCustom();
        Class<?> classInfo = classLoaderCustom.loadClass("java.util.Collection");
        PrintClassInfo(classInfo);
        System.out.println("\n\n");
        classInfo = classLoaderCustom.loadClass("Main");
        PrintClassInfo(classInfo);
    }

    public static void PrintClassInfo(Class<?> classInfo){
        System.out.println("Name: "+classInfo.getName());
        if(classInfo.getPackage()!=null){
            System.out.println("Package name: "+classInfo.getPackage().getName());
        }
        System.out.println("Superclasses: ");
        Class<?> superClass = classInfo.getSuperclass();
        while(superClass != null) {
            System.out.println(superClass.getName());
            superClass = superClass.getSuperclass();
        }

        System.out.println("Interfaces: ");
        Class<?>[] interfaces = classInfo.getInterfaces();
        for (Class<?> curInterface : interfaces) {
            System.out.println(curInterface.getName());
        }

        System.out.println("Fields: ");
        Field[] fields = classInfo.getDeclaredFields();
        for (Field field : fields) {
            System.out.println(field);
        }

        System.out.println("Constructors: ");
        Constructor<?>[] constructors = classInfo.getConstructors();
        for (Constructor<?> constructor : constructors) {
            System.out.println(constructor);
        }

        System.out.println("Methods: ");
        Method[] methods = classInfo.getMethods();
        for (Method method : methods) {
            System.out.println(method);
        }
    }
}