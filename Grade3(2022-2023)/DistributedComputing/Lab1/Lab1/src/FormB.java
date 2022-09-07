import javax.swing.*;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class FormB {
    private JPanel panel;
    private JSlider slider;
    private JButton start1Button;
    private JButton stop1Button;
    private JButton start2Button;
    private JButton stop2Button;

    private Thread th1;
    private Thread th2;

    private int semaphore = 0;

    public FormB() {
        start1Button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if(semaphore>0){
                    JOptionPane.showMessageDialog(null, "Occupied by thread 2");
                    return;
                }

                th1 = new Thread(()->{
                    while(true){
                        synchronized (slider){
                            if(slider.getValue()<90){
                                slider.setValue(slider.getValue()+10);
                            }else if(slider.getValue()>90){
                                slider.setValue(slider.getValue()-10);
                            }

                            try {
                                Thread.sleep(100);
                            } catch (InterruptedException ex) {
                                throw new RuntimeException(ex);
                            }
                        }
                    }
                });

                th1.start();
                th1.setPriority(1);
                semaphore = 1;
                stop1Button.setEnabled(true);
            }
        });

        start2Button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if(semaphore>0){
                    JOptionPane.showMessageDialog(null, "Occupied by thread 1");
                    return;
                }

                th2 = new Thread(()->{
                    while (true){
                        synchronized (slider){
                            if(slider.getValue()<10){
                                slider.setValue(slider.getValue()+10);
                            }else if(slider.getValue()>10){
                                slider.setValue(slider.getValue()-10);
                            }
                            try {
                                Thread.sleep(100);
                            } catch (InterruptedException ex) {
                                throw new RuntimeException(ex);
                            }
                        }
                    }

                });
                th2.start();
                th2.setPriority(10);
                semaphore = 1;
                stop2Button.setEnabled(true);
            }
        });

        stop1Button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                th1.stop();
                semaphore = 0;
                stop1Button.setEnabled(false);
            }
        });

        stop2Button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                th2.stop();
                semaphore = 0;
                stop2Button.setEnabled(false);
            }
        });
    }

    public static void main(String[] args) {
        JFrame frame = new JFrame("Lab1 b");

        frame.setContentPane(new FormB().panel);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(500, 500);
        frame.pack();
        frame.setVisible(true);
    }
}
