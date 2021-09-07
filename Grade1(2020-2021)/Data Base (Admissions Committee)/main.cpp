#include "stdafx.h"
#include <iostream>

//structs
struct abit_info
{
	unsigned int id;
	unsigned int id_speciality;
	char name[20];
	int marksumm;
	bool isDeleted=false;
};

struct speciality_info
{
	unsigned int id;
	char title[20];
	char subjects[3][20];
	int size;
	bool isDeleted = false;
};

//additional functions
bool IsEqual(char str1[], const char str2[]) {
	int i = 0;
	while (str1[i] != '\0' || str2[i] != '\0') {
		if (str1[i] != str2[i]) {
			return false;
		}
		i++;
	}
	if (str1[i] == '\0' && str2[i] == '\0') {
		return true;
	}
	else {
		return false;
	}
}

void ResaveDB() {
	FILE *fp;
	fopen_s(&fp, "../db.dat", "rb");
	if (!fp) {
		return;
	}
	FILE *fp2;
	fopen_s(&fp2, "../db_copy.dat", "wb");
	fseek(fp, 0, SEEK_SET);

	abit_info part;
	fread(&part, sizeof(part), 1, fp);
	while (!feof(fp)) {
		if (!part.isDeleted) {
			fwrite(&part, sizeof(part), 1, fp2);
		}
		fread(&part, sizeof(part), 1, fp);
	}

	fclose(fp);
	fclose(fp2);
	remove("../db.dat");
	rename("../db_copy.dat", "../db.dat");
}

void ResaveDBSpeciality() {
	FILE *fp;
	fopen_s(&fp, "../dbspeciality.dat", "rb");
	if (!fp) {
		return;
	}
	FILE *fp2;
	fopen_s(&fp2, "../dbspeciality_copy.dat", "wb");
	fseek(fp, 0, SEEK_SET);

	speciality_info part;
	fread(&part, sizeof(part), 1, fp);
	while (!feof(fp)) {
		if (!part.isDeleted) {
			fwrite(&part, sizeof(part), 1, fp2);
		}
		fread(&part, sizeof(part), 1, fp);
	}

	fclose(fp);
	fclose(fp2);
	remove("../dbspeciality.dat");
	rename("../dbspeciality_copy.dat", "../dbspeciality.dat");
}

void DeleteById(int id) {
	FILE *fp;
	fopen_s(&fp, "../db.dat", "r+b");

	if (!fp) {
		std::cout << "List of applicants is empty \nEnter 'add' to register an applicant \n";
		return;
	}
	fseek(fp, 0, SEEK_END);
	long pos = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	int count = pos / sizeof(abit_info);
	abit_info* part = new abit_info[count];

	fread(part, sizeof(abit_info), count, fp);
	for (int i = 0; i <= count; i++) {
		if (part[i].id == id) {
			part[i].isDeleted = true;
			fseek(fp, sizeof(abit_info)*(i), SEEK_SET);
			fwrite(&part[i], sizeof(part[i]), 1, fp);
			delete[] part;
			fclose(fp);
			return;
		}
	}
	std::cout << "Applicant with this id isn't exist\n";
	delete[] part;
	fclose(fp);
}

void DeleteSpecialityById(int id) {
	FILE *fp;
	fopen_s(&fp, "../dbspeciality.dat", "r+b");

	if (!fp) {
		std::cout << "List of specialities is empty \n";
		return;
	}
	fseek(fp, 0, SEEK_END);
	long pos = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	int count = pos / sizeof(speciality_info);
	speciality_info* part = new speciality_info[count];

	fread(part, sizeof(speciality_info), count, fp);
	for (int i = 0; i <= count; i++) {
		if (part[i].id == id) {
			part[i].isDeleted = true;
			fseek(fp, sizeof(speciality_info)*(i), SEEK_SET);
			fwrite(&part[i], sizeof(part[i]), 1, fp);
			delete[] part;
			fclose(fp);
			return;
		}
	}
	std::cout << "Speciality with this id isn't exist\n";
	delete[] part;
	fclose(fp);
}

void DeleteAllAbitInSpeciality(int id) {
	FILE *fp;
	fopen_s(&fp, "../db.dat", "r+b");

	if (!fp) {
		return;
	}
	fseek(fp, 0, SEEK_END);
	long pos = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	int count = pos / sizeof(abit_info);
	abit_info* part = new abit_info[count];

	fread(part, sizeof(abit_info), count, fp);
	for (int i = 0; i <= count; i++) {
		if (part[i].id_speciality == id) {
			part[i].isDeleted = true;
			fseek(fp, sizeof(abit_info)*(i), SEEK_SET);
			fwrite(&part[i], sizeof(part[i]), 1, fp);
		}
	}
	delete[] part;
	fclose(fp);
}

speciality_info* GetSpecialityById(int id) {
	FILE *fp;
	fopen_s(&fp, "../dbspeciality.dat", "rb");
	if (!fp) {
		std::cout << "List of specialities is empty \n";
		return NULL;
	}
	fseek(fp, 0, SEEK_END);
	long pos = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	int count = pos / sizeof(speciality_info);
	speciality_info* part = new speciality_info[count];
	fread(part, sizeof(speciality_info), count, fp);
	

	for (int i = 0; i <count; i++) {
		if (part[i].id == id) {
			fclose(fp);
			if (part[i].isDeleted) {
				std::cout << "Speciality with this id doesn't exist\nEnter 'specialities' to get list of specialities \n";
				return NULL;
			}
			return &part[i];
		}
	}
	delete[] part;
	fclose(fp);
	std::cout << "Speciality with this id doesn't exist\nEnter 'specialities' to get list of specialities \n";
	return NULL;
}

int GetLastAbitId() {
	unsigned int id = 0;
	abit_info part;
	FILE *fp;
	fopen_s(&fp, "../db.dat", "rb");
	if (fp) {
		fseek(fp, 0, SEEK_END);
		long pos = ftell(fp);
		if (pos>0)
		{
			fseek(fp, pos - sizeof(abit_info), SEEK_SET);
			fread(&part, sizeof(part), 1, fp);
			id = part.id + 1;
		}
		fclose(fp);
	}
	
	return id;
}

int GetLastSpecialityId() {
	unsigned int id = 0;
	speciality_info part;
	FILE *fp;
	fopen_s(&fp, "../dbspeciality.dat", "rb");
	if (fp) {
		fseek(fp, 0, SEEK_END);
		long pos = ftell(fp);
		if (pos>0)
		{
			fseek(fp, pos - sizeof(speciality_info), SEEK_SET);
			fread(&part, sizeof(part), 1, fp);
			id = part.id + 1;
		}
		fclose(fp);
	}

	return id;
}

//database functions
bool addApplicant(int id) {
	FILE *fp;
	fopen_s(&fp, "../db.dat", "ab");
	fseek(fp, 0, SEEK_END);
	abit_info part;

	int summ_marks = 0;
	int n;
	part.id = id;

	std::cout << "Enter id of the speciality ";
	std::cin >> part.id_speciality;
	if (std::cin.fail()) {
		std::cout << "Error, the entered value is not a number\n";
		std::cin.clear();
		std::cin.ignore();
		fclose(fp);
		return 0;
	}

	speciality_info* speciality = GetSpecialityById(part.id_speciality);
	if (!speciality) {
		fclose(fp);
		return 0;
	}

	std::cout << "Enter name of the applicant ";
	std::cin.ignore();
	std::cin.getline(part.name, 20);

	std::cout << "Enter "<<speciality->subjects[0] <<" grade ";
	std::cin >> n;
	if (std::cin.fail()) {
		std::cout << "Error, the entered value is not a number\n";
		std::cin.clear();
		std::cin.ignore();
		fclose(fp);
		return 0;
	}

	summ_marks += n;

	std::cout << "Enter " << speciality->subjects[1] << " grade ";
	std::cin >> n;
	if (std::cin.fail()) {
		std::cout << "Error, the entered value is not a number\n";
		std::cin.clear();
		std::cin.ignore();
		fclose(fp);
		return 0;
	}

	summ_marks += n;

	std::cout << "Enter " << speciality->subjects[2] << " grade ";
	std::cin >> n;
	if (std::cin.fail()) {
		std::cout << "Error, the entered value is not a number\n";
		std::cin.clear();
		std::cin.ignore();
		fclose(fp);
		return 0;
	}
	summ_marks += n;

	std::cout << "Enter Grade Point Average ";
	std::cin >> n;

	if (std::cin.fail()) {
		std::cout << "Error, the entered value is not a number\n";
		std::cin.clear();
		std::cin.ignore();
		fclose(fp);
		return 0;
	}
	summ_marks += n;
	part.marksumm = summ_marks;

	fwrite(&part, sizeof(part), 1, fp);

	fclose(fp);
	std::cout << "The applicant is successfully registered \n\n!!!!!ID of the applicant is "<<id<<" REMEMBER IT!!!!\n\nEnter 'top' to see the list of candidates for admission \nIf you do not see the name of the applicant, then he did not pass on the sum of grades\n";
	return 1;
}

bool addSpeciality(int id) {
	FILE *fp;
	fopen_s(&fp, "../dbspeciality.dat", "ab");
	fseek(fp, 0, SEEK_END);
	speciality_info part;

	part.id = id;
	std::cout << "Enter title of the speciality ";
	std::cin.ignore();
	std::cin.getline(part.title, 20);
	std::cout << "Enter first examination subject ";
	std::cin.getline(part.subjects[0], 20);
	std::cout << "Enter second examination subject ";
	std::cin.getline(part.subjects[1], 20);
	std::cout << "Enter third examination subject ";
	std::cin.getline(part.subjects[2], 20);
	std::cout << "Enter total size of places ";
	std::cin >> part.size;
	if (std::cin.fail()) {
		std::cout << "Error, the entered value is not a number\n";
		std::cin.clear();
		std::cin.ignore();
		fclose(fp);
		return 0;
	}

	fwrite(&part, sizeof(part), 1, fp);

	fclose(fp);
	std::cout << "The speciality is successfully added\n";
	return 1;
}

void GetTop()
{
	int speciality_id;
	std::cout << "Enter speciality id ";
	std::cin >> speciality_id;
	if (std::cin.fail()) {
		std::cout << "Error, the entered value is not a number\n";
		std::cin.clear();
		std::cin.ignore();
		return;
	}
	speciality_info* spec = GetSpecialityById(speciality_id);
	if (!spec) {
		return;
	}

	FILE *fp;
	fopen_s(&fp, "../db.dat", "rb");
	if (!fp) {
		std::cout << "List of applicants is empty\nEnter 'add' to register an applicant \n";
		return;
	}
	fseek(fp, 0, SEEK_END);
	long pos = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	int count = pos / sizeof(abit_info);
	abit_info* part = new abit_info[count];
	fread(part, sizeof(abit_info), count, fp);

	int *marks=new int[count];
	int *nums=new int[count];

	int cnt = 0;

	for (int i = 0; i < count; i++) {
		if (part[i].id_speciality != speciality_id || part[i].isDeleted) continue;
		marks[cnt] = part[i].marksumm;
		nums[cnt] = i;
		cnt++;
	}
	if (cnt == 0) {
		std::cout<< "List of applicants is empty\nEnter 'add' to register an applicant \n";
		delete[] part;
		fclose(fp);
		return;
	}

	for (int i = 0; i < cnt; i++) {
		for (int i = 0; i < cnt-1; i++) {
			if (marks[i] < marks[i + 1]) {
				int t = marks[i];
				marks[i] = marks[i + 1];
				marks[i + 1] = t;
				t = nums[i];
				nums[i] = nums[i + 1];
				nums[i + 1] = t;
			}
		}
	}
	
	
	
	int num = spec->size;
	if (cnt < num) {
		num = cnt;
	}
	for (int i = 0; i<num; i++)
		std::cout << "Name: " << part[nums[i]].name << " Marks: " << part[nums[i]].marksumm << std::endl;
	
	
	delete[] part;
	fclose(fp);
}

void printSpecialities() {
	FILE *fp;
	fopen_s(&fp, "../dbspeciality.dat", "rb");
	if (!fp) {
		std::cout << "List of specialities is empty \nEnter 'addspeciality' to add a speciality \n";
		return;
	}
	fseek(fp, 0, SEEK_END);
	long pos = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	int count = pos / sizeof(speciality_info);
	speciality_info* part = new speciality_info[count];
	fread(part, sizeof(speciality_info), count, fp);

	bool flag = true;
	for (int i = 0; i <count; i++) {
		if (part[i].isDeleted) { continue; }
		flag = false;
		std::cout << "Id: " << part[i].id << "  Title: " << part[i].title << "  Size: " << part[i].size<<"\n";
	}
	if (flag) {
		std::cout << "List of specialities is empty \nEnter 'addspeciality' to add a speciality \n";
	}
	delete[] part;
	fclose(fp);
}

int main()
{
	unsigned int abit_id = GetLastAbitId();
	unsigned int speciality_id = GetLastSpecialityId();
	

	char info[] = "Enter 'quit' to exit \nEnter 'help' to see a list of available functions \nEnter 'add' to register an applicant \nEnter 'top' to see the list of candidates for admission \nEnter 'delete' to delete applicant by id\nEnter 'specialities' to get list of specialities \nEnter 'addspeciality' to add a speciality \nEnter 'deletespeciality' to delete a speciality by id \n";
	std::cout << info;
	char userinput[20];

	while(true){
		std::cout << ">>> ";
		std::cin >> userinput;
		if (IsEqual(userinput,"quit")) {
			ResaveDB();
			ResaveDBSpeciality();
			break;
		}
		else if(IsEqual(userinput,"help")){
			std::cout << info;
		}
		else if (IsEqual(userinput, "add")) {
			if(addApplicant(abit_id)){ abit_id++; }
		}
		else if (IsEqual(userinput, "top")) {
			GetTop();
		}
		else if (IsEqual(userinput, "delete")) {
			int id;
			std::cout << "Enter id of the applicant ";
			std::cin >> id;
			if (std::cin.fail()) {
				std::cout << "Error, the entered value is not a number\n";
				std::cin.clear();
				std::cin.ignore();
			}
			else {
				DeleteById(id);
			}
		}
		else if (IsEqual(userinput, "addspeciality")) {
			if(addSpeciality(speciality_id)){ speciality_id++; }
			
		}
		else if (IsEqual(userinput, "deletespeciality")) {
			int id;
			std::cout << "Enter id of the speciality ";
			std::cin >> id;
			if (std::cin.fail()) {
				std::cout << "Error, the entered value is not a number\n";
				std::cin.clear();
				std::cin.ignore();
			}
			else {
				DeleteSpecialityById(id);
				DeleteAllAbitInSpeciality(id);
			}
		}
		else if (IsEqual(userinput, "specialities")) {
			printSpecialities();
		}
		else {
			std::cout << "This command isn't exist, enter 'help' to see a list of available functions \n";
		}
	}

    return 0;
}

