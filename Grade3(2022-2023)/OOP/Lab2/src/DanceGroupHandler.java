import jdk.internal.org.xml.sax.SAXException;
import org.xml.sax.Attributes;
import org.xml.sax.helpers.DefaultHandler;

import java.util.ArrayList;

public class DanceGroupHandler extends DefaultHandler {
    public ArrayList<DanceGroup> danceGroups;
    private StringBuilder elementValue;

    @Override
    public void characters(char[] ch, int start, int length) {
        if (elementValue == null) {
            elementValue = new StringBuilder();
        } else {
            elementValue.append(ch, start, length);
        }
    }

    @Override
    public void startDocument() {
        danceGroups = new ArrayList<DanceGroup>();
    }

    @Override
    public void startElement(String uri, String lName, String qName, Attributes attr) {
        switch (qName) {
            case "Dance":
                danceGroups = new ArrayList<DanceGroup>();
                break;
            case "DanceGroup":
                danceGroups.add(new DanceGroup());
                break;
            case "Type":
            case "Scene":
            case "NumberOfDancers":
            case "Music":
            case "Number":
            case "Name":
            case "Age":
            case "ExperienceYears":
                elementValue = new StringBuilder();
                break;
            case "Dancers":
                latestDanceGroup().dancers.add(new Dancer());
                break;
        }
    }

    @Override
    public void endElement(String uri, String localName, String qName) {
        switch (qName) {
            case "Type":
                latestDanceGroup().type = Type.valueOf(elementValue.toString());
                break;
            case "Scene":
                latestDanceGroup().scene = Scene.valueOf(elementValue.toString());
                break;
            case "NumberOfDancers":
                latestDanceGroup().numberOfDancers = NumberOfDancers.valueOf(elementValue.toString());
                break;
            case "Music":
                latestDanceGroup().music = Music.valueOf(elementValue.toString());
                break;
            case "Number":
                latestDanceGroup().number = Integer.parseInt(elementValue.toString());
                break;

            case "Name":
                latestDancer().name = elementValue.toString();
                break;
            case "Age":
                latestDancer().age = Integer.parseInt(elementValue.toString());
                break;
            case "ExperienceYears":
                latestDancer().experienceYears = Integer.parseInt(elementValue.toString());
                break;
        }
    }

    private DanceGroup latestDanceGroup() {
        return danceGroups.get(danceGroups.size() - 1);
    }

    private Dancer latestDancer() {
        return latestDanceGroup().dancers.get(latestDanceGroup().dancers.size() - 1);
    }
}
