import javax.swing.*;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class FormA {
    private JSlider slider;
    private JSpinner spinner1;
    private JSpinner spinner2;
    private JButton startButton;
    private JPanel panel;
    private Thread th1;
    private Thread th2;

    public FormA() {
        startButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
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


				th1.setDaemon(true);
                th1.start();
                th1.setPriority(1);
				th2.setDaemon(true);
                th2.start();
                th2.setPriority(1);

                startButton.setEnabled(false);
                spinner1.setEnabled(true);
                spinner2.setEnabled(true);
            }
        });

        spinner1.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                th1.setPriority((Integer) spinner1.getValue());
            }
        });

        spinner2.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                th2.setPriority((Integer) spinner2.getValue());
            }
        });
    }

    public static void main(String[] args) {
        JFrame frame = new JFrame("Lab1 a");

        frame.setContentPane(new FormA().panel);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(500, 500);
        frame.pack();
        frame.setVisible(true);
    }

    private void createUIComponents() {
        SpinnerNumberModel sm1 = new SpinnerNumberModel(1, 1, 100, 1);
        SpinnerNumberModel sm2 = new SpinnerNumberModel(1, 1, 100, 1);
        spinner1 = new JSpinner(sm1);
        spinner2 = new JSpinner(sm2);
    }
}
