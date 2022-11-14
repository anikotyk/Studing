import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

public class ClassLoaderCustom extends ClassLoader {
    @Override
    public Class<?> loadClass(String className) throws ClassNotFoundException {
        if (className.startsWith("java.")) {
            return super.loadClass(className);
        }
        else
        {
            try {
                InputStream inputStream = getClass().getClassLoader().getResourceAsStream(className.replace(".", "/") + ".class");
                ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                int data = inputStream.read();

                while (data != -1) {
                    byteArrayOutputStream.write(data);
                    data = inputStream.read();
                }

                return defineClass(className, byteArrayOutputStream.toByteArray(), 0, byteArrayOutputStream.toByteArray().length);

            } catch (IOException e) {
                throw new RuntimeException(e);
            }

        }
    }
}