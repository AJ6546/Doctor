using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disease : MonoBehaviour, ISaveable
{
    [SerializeField] string disease; // Disease patient has
    
    // List of all diseases in the game
    List<string> diseaseLibrary = new List<string>()
    {
        "HYPERTENSION",
        "HIGH CHOLESTROL",
        "MALARIA",
        "JAUNDICE",
        "FLU",
        "BRONCHITIS",
        "BLOOD CLOTS",
        "ANGINA",
        "BRAIN ANEURYSM",
        "DEEP VEIN THROMBOSIS",
        "ARRHYTHMIA",
        "HEART ATTACK",
        "STROKE",
        "PULMONARY EMBOLISM",
        "POSTPHLEBITIC SYNDROME",
        "CHRONIC BRONCHITIS",
        "H1N1",
        "EMPHYSEMA",
        "PNEUMONIA",
        "PNEUMOTHORAX",
        "LUNG CANCER",
        "TUBERCULOSIS",
        "RESPIRATORY FAILURE",
        "CHRONIC STRESS",
        "NERVOUS BREAKDOWN",
        "DEMENTIA",
        "ALZHEIMER'S",
        "BURNOUT",
        "EPILEPTIC SEIZURE",
        "MAJOR DEPRESSION",
        "PARKINSON'S",
        "HEMIPARESIS",
        "POTASSIUM DEFICIENCY",
        "CHRONIC MUSCLE CRAMP",
        "FIBROMYALGIA",
        "MUSCULAR DYSTROPHY",
        "NECROTIZING FASCIITIS",
        "SKIN CANCER",
        "ARTHRITIS",
        "RICKETS",
        "OSTEOPOROSIS",
        "FREQUENT BONE FRACTURE",
        "BONE DEATH",
        "LEUKEMIA",
        "PAGET'S",
        "CONSTIPATION",
        "GASTROENTERITIS",
        "SALMONELLA INFECTION",
        "CIRRHOSIS",
        "HEMORRHOIDS",
        "IRRITABLE BOWEL",
        "CROHN'S",
        "COLON CANCER",
        "TYPE 1 DIABETES",
        "TYPE 2 DIABETES",
        "GALLSTONES",
        "PANCREATITIS",
        "KIDNEY STONES",
        "KIDNEY FAILURE",
        "CHRONIC KIDNEY FAILURE",
        "LUPUS NEPHRITIS",
        "KIDNEY INFECTION",
        "SEPSIS",
        "URINARY INCONTINENCE",
        "KIDNEY CYST",
        "URINARY TRACT INFECTION",
        "ASTHMA",
        "PHOTOSENSITIVITY",
        "INSOMNIA",
        "CHRONIC FATIGUE",
        "IMMUNODEFICIENCY",
        "ADDISON'S",
        "LUPUS",
        "SARCOIDOSIS",
    };

    // Dictionary of disease - description
    Dictionary<string,string> diseaseDecription = new Dictionary<string, string>(){
         { "HYPERTENSION","High Blood Pressure is a common condition in which the force of the blood against your artery walls is high enough that it may eventually cause health problems, such as heart disease.\n\n Treatments: Alpha Blockers (15 Days)"}
        ,{ "HIGH CHOLESTROL","When you have high cholesterol, you may develop fatty deposits in your blood vessels. Eventually, these deposits make it difficult for enough blood to flow through your arteries.\n\n Treatments: Strict Excercise, Staints (6 Days)"}
        ,{ "MALARIA","Malaria is caused by parasites that enter your body through the bite of an infected mosquito. This sometimes fatal disease happens in hot and humid places, like Africa.\n\n Treatments: The type of parasite will determine what type of medication you take and how long you take it. Artemisinin drugs are the best treatment for Plasmodium falciparum malaria."}
        ,{ "JAUNDICE","Jaundice is a condition in which the skin, whites of the eyes and mucous membranes turn yellow because of a high level of bilirubin, a yellow-orange bile pigment. Jaundice has many causes, including hepatitis, gallstones and tumors. In adults, jaundice usually doesn't need to be treated."}
        ,{ "FLU", "Influenza is a viral infection that attacks your respiratory system - your nose, throat and lungs.\n\nTreatments: Rest" }
        ,{ "BRONCHITIS", "Bronchitis is an inflammation of the lining of your bronchial tubes, which carry air to and from your lungs. Bronchitis may either be acute or chronic.\n\n Treatments: Antibiotics (15 Days)"}
        ,{ "BLOOD CLOTS", "Blood Clots are gel-like clumps of blood. Some blood clots form inside your veins without a good reason and don't dissolve naturally.\n\n Treatments: Anticoagulants  (15 Days)"}
        ,{ "ANGINA", "Angina is a type of chest pain caused by reduced blood flow to the heart muscle. Angina is a symptom of coronary artery disease. Angina is typically described as squeezing, pressure, heaviness, tightness or pain in your chest.\n\n Treatments: Asprin (7 Days), Nitroglycerin (7 Days)"}
        ,{ "BRAIN ANEURYSM","A brain aneurysm is a bulge or ballooning in a blood vessel in the brain. It often looks like a berry hanging on a stem. A brain aneurysm can leak or rupture, causing bleeding into the brain.\n\n Treatments: Surgical Clipping (16 Days)"}
        ,{ "DEEP VEIN THROMBOSIS", "Deep Vein Thrombosis occurs when a blood clot (thrombus) forms in one or more of the deep veins in your body, usually in your legs. Deep Vein Thrombosis can cause leg pain or swelling, but may occur without any symptoms.\n\n Treatments: Clot Busters (9 Days)"}
        ,{ "ARRHYTHMIA","Heart rhythm problems (heart arrhythmias) occur when the electrical impulses in your heartbeats don't work properly, causing your heart to beat too fast, too slow or irregularly.\n\n Treatments: Pacemaker"}
        ,{ "HEART ATTACK","A heart attack usually occurs when a blood clot blocks the flow of blood through a coronary artery- a blood vessel that feeds blood to a part of the heart muscle.\n\n Treatments: Nitroglycerin (2 Days)"}
        ,{ "STROKE","A stroke occurs when the blood supply to part of your brain is interrupted or severly reduced, depriving brain tissue of oxygen and food. Within minutes, brain cells begin to die.\n\n Treatments: Aspirin (2 Days), Thrombolysis (11 Days)"}
        ,{ "PULMONARY EMBOLISM","Pulmonary Embolism is the blockage in one of the Pulmonary Arteries in your lungs. In most cases, pulmonary embolism caused by blood clots that travel to the lungs from the legs or, rarely other parts of the body (Deep Vein Thrombosis).\n\n Treatments: Anticoagulants (9 Days), Clot Busters (5 Days)"}
        ,{ "POSTPHLEBITIC SYNDROME", "Postphlebitic syndrome is caused by damage to your veins from a blood clot. This damage reduces blood flow in the affected areas.\n\n Treatments: Postphlebitic Therapy (22 Days)"}
        ,{ "CHRONIC BRONCHITIS","Bronchitis is an inflammation of the lining of your bronchial tubes, which carry air to and from your lungs. Bronchitis may either be acute or chronic.\n\n Treatments: Pulmonary Rehabilitation (15 Days)"}
        ,{ "H1N1","Technically, the term swine flu refers to influenza in pigs. Occasionally, pigs transmit influenza viruses to people, mainly to hog farm workers and veterinarians.\n\n Treatments: Rest (15 Days), Antiviral drugs (7 Days)"}
        ,{ "EMPHYSEMA","Emphysema occurs when the air sacs in your lungs are gradually destroyed, making you progressively more short of breath.Smoking is the leading cause of emphysema.\n\n Treatments: Bronchodilators (15 Days)"}
        ,{ "PNEUMONIA","Pneumonia is an infection that inflames the air sacs in one or both lungs. The air sacs may fill with fluid or pus, causing cough with phlegm or pus, fever, chills and difficulty breathing.\n\n Treatments: Antibiotics (15 Days)"}
        ,{ "PNEUMOTHORAX","A pneumothorax is a collapsed lung. Pneumothorax occurs when air leaks into the space between your lungs and chest wall. This air pushes on the outside of your lung and it makes it collapse.\n\n Treatments: Thorasostomy (2 Days)"}
        ,{ "LUNG CANCER", "Lung cancer is a type of cancer that begins in the lungs. Your lungs are two spongy organs in your chest that take in oxygen when you inhale and release carbon dioxide when you exhale.\n\n Treatments: Chemotherapy (33 Days),  Wedge Resection (16 Days)"}
        ,{ "TUBERCULOSIS", "Tuberculosis is a serious infectious disease that mainly infects your lungs. The bacteria that causes tuberculosis are spread from one person to another through tiny droplets released into the air via coughs and sneezes.\n\n Treatments: Isoniazid (33 Days)"}
        ,{ "RESPIRATORY FAILURE","Acute Respiratory Stress Syndrome occurs when fluid builds up in the tiny, elastic air sacs (Alveoli) in your lungs. More fluid in your lungs means less oxygen can reach your blood stream. this deprives your organs of the oxygen they need to function.\n\n Treatments: Mechanical Ventilation (5 Days), Oxygen Therapy (5 Days)"}
        ,{ "CHRONIC STRESS", "Chronic stress is the response to emotional pressure suffered for a prolonged period of time. Stress symptoms include anxiety, depression, digestive problems, heart disease and many more.\n\n Treatments: Stress Management Therapy (30 Days), AntiDepressants (10 Days)"}
        ,{ "NERVOUS BREAKDOWN", "The term nervous breakdown is sometimes used to describe a stressful situation in which someone becomes temporarily unable to function normally in day-to-day life.\n\n Treatments: Psychiatrist Counseling (33 Days), Anxiolytics (7 Days)"}
        ,{ "DEMENTIA","Dementia is an uncommon disorders that primarily affect the frontal and temporal lobes of the brain- the areas generally associated with personality, behavior and language.\n\n Treatments: Cholinesterase Inhibitors (16 Days)"}
        ,{ "ALZHEIMER'S","Alzheimer's Disease is a progressive disease that destroys memory and other important mental functions.\n\n Treatments: Cholinesterase Inhibitors (15 Days)"}
        ,{ "BURNOUT","Job burnout is a special type of job stress- a state of physical, emotional or mental exhaustion combined with doubts about your competence and the value of your work.\n\n Treatments: AntiDepressants (7 Days)"}
        ,{ "EPILEPTIC SEIZURE","Epilepsy is a central nervous system disorder (neurological disorder) in which the nerve cell activity in your brain is disturbed, causing a seizure during which you experience abnormal behavior, symptoms and sensations, including loss of consciousness.\n\n Treatments: Anticonvulsants (15 Days)"}
        ,{ "MAJOR DEPRESSION","Depression is a mood disorder that caused a persistent feeling of sadness and loss of interest. Major depression affects how you feel, think and behave and can lead to a variety of emotional and physical problems.\n\n Treatments: Selective Serotonin Reuptake Inhibitors (22 Days), AntiDepressants (22 Days)"}
        ,{ "PARKINSON'S", "Parkinson's disease is a progressive disorder of the nervous system that affects movement. While a tremor may be the most well-known sign of Parkinson's disease. The disorder also commonly causes stiffness or slowing of movement.\n\n Treatments: Carbidopa/levodopa (33 Days)"}
        ,{ "HEMIPARESIS", "Hemiparesis is the weakness of the entire left or right side of the body.\n\n Treatments: Physiotherapy (33 Days)"}
        ,{ "POTASSIUM DEFICIENCY", "Low potassium (hypokalemia) refers to a lower than normal level of potassium in your bloodstream. Potassium is a chemical (electrolyte) that is critical to the proper functioning of nerve and muscles cells, particularly muscle cells.\n\n Treatments: Potassium Supplements (15 Days)"}
        ,{ "CHRONIC MUSCLE CRAMP","Overuse of a muscle, dehydration, muscle strain or simply holding a position for a prolonged period of time may result in a muscle cramp.\n\n Treatments: Physiotherapy (23 Days), Aspirin (7 Days)"}
        ,{ "FIBROMYALGIA","Fibromyalgia is a disorder characterized by widespread musculoskeletal pain accompanied by fatigue, sleep, memory and mood issues. Fibromyalgia amplifies painful sensations by affecting the way your brain processes pain signals.\n\n Treatments: Aspirin  (15 Days), Antidepressants (11 Days), Anticonvulsants (15 Days)"}
        ,{ "MUSCULAR DYSTROPHY", "Muscular dystrophy is a group of genetic diseases in which muscle fibers are unusually susceptible to damage.\n\n Treatments: Corticosteroids (7 Days)"}
        ,{ "NECROTIZING FASCIITIS","Necrotizing Fasciitis is often referred to as flesh eating bacteria. Necrotizing Fasciitis spreads aggressively in an infected person and causes tissue death at the infection site and beyond.\n\n Treatments: Anti-biotics (7 Days), Amputation (5 Days)"}
        ,{ "SKIN CANCER", "Skin Cancer - the abnormal growth of skin cells - most often develops on skin exposed to the sun. But this common form of cancer can also occur on areas of your skin not ordinarily exposed to sunlight.\n\n Treatments: Excisional Surgery (7 Days), Chemotherapy (23 Days)"}
        ,{ "ARTHRITIS","Arthritis is inflammation of one or more of your joints. The main symptoms of arthritis are joint pain and stiffness, which typically worsen with age.\n\n Treatments: Analgesics (15 Days), Anti-inflammatory Drugs (15 Days)"}
        ,{ "RICKETS","Rickets is the softening and weakening of bones in children, usually because of an extreme and prolonged vitamin deficiency.\n\n Treatments: Calcium/Vitamin D Supplements (30 Days)"}
        ,{ "OSTEOPOROSIS", "Osteoporosis causes bones to become weak and brittle- so brittle that a fall or even mild stresses like bending over or coughing can cause a fracture. Osteoporosis-related fractures most commonly occur in the hip, wrist or spine.\n\n Treatments: Biphosphonates (23 Days)"}
        ,{ "FREQUENT BONE FRACTURE","Weakened bones. Conditions such as osteoporosis can weaken your bones and make it easier for stress fractures to occur.\n\n Treatments: Plaster Cast (33 Days)"}
        ,{ "BONE DEATH", "Avascular necrosis is the death of bone tissue due to a lack of blood supply. Also called osteonecrosis, avascular necrosis can lead to tiny breaks in the bone and the bone's eventual collapse.\n\n Treatments: Biphosphonates (23 Days), Anti-inflammatory Drugs (27 Days)"}
        ,{ "LEUKEMIA","Leukemia is the cancer of the body's blood-forming tissues, including the bone marrow and the lymphatic systems.\n\n Treatments: Chemotherapy (20 Days)"}
        ,{ "PAGET'S","Paget's disease of bone interferes with your body's normal recycling process, in which new bone tissue gradually replaces old bone tissue.\n\n Treatments: Biphosphonates (3 Days)"}
        ,{ "CONSTIPATION","Chronic constipation is infrequent bowel movements or difficult passage of stools that persists for several weeks or longer.\n\n Treatments: Fiber supplements (15 Days)"}
        ,{ "GASTROENTERITIS","Viral gastroenteritis (stomach flu) is an intestinal infection marked by watery diarrhea, abdominal cramps, nausea or vomiting, and sometimes fever.\n\n Treatments: Rest (15 Days)"}
        ,{ "SALMONELLA INFECTION","Salmonella infection is a common bacterial disease that affects the intestinal tract.\n\n Treatments: Anti-diarrheals (22 Days), Anti-biotics (5 Days)"}
        ,{ "CIRRHOSIS","Cirrhosis is scarring of the liver caused by many forms of liver diseases and conditions, such as hepatitis and chronic alcohol abuse.\n\n Treatments: Alcohol Dependance Therapy (33 Days), Liver Transplant (33 Days)"}
        ,{ "HEMORRHOIDS","Hemorrhoids are swollen and inflamed veins in your anus and lower rectum.\n\n Treatments: Rubber Band Ligation (5 Days)"}
        ,{ "IRRITABLE BOWEL","Irritable bowel syndrome commonly causes cramping, abdominal pain, bloating gas, diarrhea and constipation.\n\n Treatments: Strict Diet (31 Days) Fiber Supplements (15 Days)"}
        ,{ "CROHN'S","Crohn's disease is an inflammatory bowel disease (IBD). It causes inflammation of the lining of your digestive tract, which can lead to abdominal pain, severe diarrhea and even malnutrition.\n\n Treatments: Corticosteroids (23 Days), Digestive Tract Surgery (7 Days), Anti-Diarrheals (7 Days)"}
        ,{ "COLON CANCER","Colon cancer is cancer of the large intestine (colon), the lower part of your digestive system.\n\n Treatments: Bowel Resection (15 Days)"}
        ,{ "TYPE 1 DIABETES","Type 1 diabetes, once known as Juvenile Diabetes or insulin-dependent diabetes, is a chronic condition in which the pancreas produces little or no insulin, a hormone needed to allow sugar (Glucose) to enter cells to produce energy.\n\n Treatments: Strict Diet (7 Days), Insulin Injections (7 Days)"}
        ,{ "TYPE 2 DIABETES","With Type 2 diabetes, your body either resists the effects of insulin - a hormone that regulates the movement of sugar in your cells - or doesn’t produce enough insulin to maintain a normal glucose level.\n\n Treatments: Insulin Injections (15 Days)"}
        ,{ "GALLSTONES","Gallstones are hardened deposits of digestive fluid that forms in your gallbladder. Your Gallblader is a small pear-shaped organ on the right side of your abdomen, just beneath your liver.\n\n Treatments: Cholecystectomy (23 Days)"}
        ,{ "PANCREATITIS","Pancreatitis is inflammation in the Pancreas. The Pancreas is a long, flat gland that sits tucked behind the stomach in the upper abdomen.\n\n Treatments: Pancreatic Enzyme Supplements (33 Days), Pancreas Surgery (7 Days)" }
        ,{ "KIDNEY STONES","Kidney stones (renal lithiasis) are small, hard deposits that form inside your kidneys.\n\n Treatments: Alpha Blockers (16 Days)"}
        ,{ "KIDNEY FAILURE","Kidney failure occurs when your kidney suddenly become unable to filter waste products from your blood. When your kidneys lose their filtering ability, dangerous levels of wastes may accumulate and your blood's chemical makeup may get out of balance.\n\n Treatments: Temporary Hemodialysis (15 Days)" }
        ,{ "CHRONIC KIDNEY FAILURE", "Kidney failure occurs when your kidney suddenly become unable to filter waste products from your blood. When your kidneys lose their filtering ability, dangerous levels of wastes may accumulate and your blood's chemical makeup may get out of balance.\n\n Treatments: Hemodialysis (15 Days), Kidney Transplant (7 Days)"}
        ,{ "LUPUS NEPHRITIS","Lupus nephritis occurs when lupus autoantibodies affect the filtering structures (glomeruli) of your kidneys. This abnormal process results in kidney inflammation and may lead to blood in the urine.\n\n Treatments: Corticosteroids (15 Days), Hemodialysis (15 Days)"}
        ,{ "KIDNEY INFECTION","Kidney infection (pyelonephritis) is a specific type of urinary tract infection that generally begins in your urethra or bladder and travels up into your kidneys.\n\n Treatments: Antibiotics (23 Days)"}
        ,{ "SEPSIS","Sepsis is a potentially life-threatening complication of an infection. Sepsis occurs when chemicals released into the bloodstream to fight the infection trigger inflammation throughout the body.\n\n Treatments: Intravenous Antibiotics (15 Days)"}
        ,{ "URINARY INCONTINENCE","Urinary Incontinence is a common and often embarrassing problem. The severity ranges from occasionally leaking urine when you cough or sneeze to having an urge to urinate that's so sudden and strong you don't get to the toilet in time.\n\n Treatments: Bladder Training (23 Days), Anticholinergics (7 Days)"}
        ,{ "KIDNEY CYST","Kidney Cysts are round pouches of fluid that form on or in the kidneys. Kidney Cysts can be associated with serious disorders that may impair kidney functions.\n\n Treatments: Cyst Surgery (7 Days)"}
        ,{ "URINARY TRACT INFECTION","A urinary tract infection (UTI) is an infection in any part of your urinary system - your kidneys, ureters, bladder and urethra. Most infections involve the lower urinary tract - the bladder and the urethra.\n\n Treatments: Antibiotics (15 Days)"}
        ,{ "ASTHMA","Asthma is a condition in which your airways narrow and swell and produce extra mucus. This can make breathing difficult and trigger coughing, wheezing and shortness of breath.\n\n Treatments: Inhaled Corticosteroids (2 Days)"}
        ,{ "PHOTOSENSITIVITY","Photosensitivity is a term often used to describe a number of conditions in which an itchy red rash occurs on skin that's been exposed to sunlight.\n\n Treatments: Corticosteroids (15 Days)"}
        ,{ "INSOMNIA","Insomnia is a persistent disorder that can make it hard to fall asleep, hard to stay asleep or both, despite the opportunity for adequate sleep.\n\n Treatments: Relaxation Technique (23 Days), Sleeping Pills (7 Days)"}
        ,{ "CHRONIC FATIGUE","Chronic fatigue syndrome is a complicated disorder characterized by extreme fatigue that can't be explained by any underlying medical condition. The fatigue may worsen with physical or mental activity, but doesn't improve with rest.\n\n Treatments: Antidepressants (33 Days), Sleeping Pills (5 Days), Physiotherapy (23 Days)"}
        ,{ "IMMUNODEFICIENCY","Primary immunodeficiency disorders - also called primary immune disorders or primary immunodeficiency - weaken the immune system, allowing repeated infections and other health problems to occur more easily.\n\n Treatments: Immunoglobu lin Therapy (28 Days)"}
        ,{ "ADDISON'S","Addison’s Disease is a disorder that occurs when your body produces insufficient amounts of certain hormones produced by your adrenal glands.\n\n Treatments: Corticosteroids (15 Days)" }
        ,{ "LUPUS","Lupus is a chronic inflammatory disease that occurs when your body's immune system attacks your own tissues and organs. Inflammation caused by lupus can affect many different body systems - including your joints, skin, kidneys, blood cells, brain, heart, and lungs.\n\n Treatments: Immunosuppressive Drugs (33 Days), Corticosteroids (7 Days)" }
        ,{ "SARCOIDOSIS","Sarcoidosis is the growth of tiny collections of inflammatory cells (granulomas) in different parts of your body - most commonly the lungs, lymph nodes, eyes and skin.\n\n Treatments: Corticosteroids (15 Days), Immunosuppressive Drugs (23 Days)"}
    };

    // Dictionary of disease - test for disease
    Dictionary<string, string> testForDisease = new Dictionary<string, string>(){
        { "HYPERTENSION","BLOOD PRESSURE TEST" },
        { "HIGH CHOLESTROL","METABOLIC PANEL" },
        { "MALARIA","BLOOD" },
        { "JAUNDICE","URINE" },
        { "FLU","STETHESCOPE" },
        { "BRONCHITIS","STETHESCOPE" },
        { "BLOOD CLOTS","ULTRASOUND" },
        { "ANGINA", "ECG" },
        { "BRAIN ANEURYSM","MRI" },
        { "DEEP VEIN THROMBOSIS","ULTRASOUND" },
        { "ARRHYTHMIA","ECG" },
        { "HEART ATTACK","ECG" },
        { "STROKE","MRI" },
        { "PULMONARY EMBOLISM","PULMONARY ANGIOGRAM" },
        { "POSTPHLEBITIC SYNDROME","ULTRASOUND" },
        { "CHRONIC BRONCHITIS","X-RAY" },
        { "H1N1","FLU TEST" },
        { "EMPHYSEMA", "SPIROMETER" },
        { "PNEUMONIA", "BLOOD" },
        { "PNEUMOTHORAX", "X-RAY" },
        { "LUNG CANCER","BIOPSY" },
        { "TUBERCULOSIS", "BLOOD" },
        { "RESPIRATORY FAILURE","RESPIRATORY MONITORING" },
        { "CHRONIC STRESS", "EEG" },
        { "NERVOUS BREAKDOWN", "PSYCHOLOGICAL EVALUATION" },
        { "DEMENTIA", "PET SCAN" },
        { "ALZHEIMER'S","MRI" },
        { "BURNOUT","PSYCHOLOGICAL EVALUATION" },
        { "EPILEPTIC SEIZURE", "EEG" },
        { "MAJOR DEPRESSION", "PSYCHIATRIC ASSESSMENT" },
        { "PARKINSON'S",  "MRI"},
        { "HEMIPARESIS", "NEUROLOGICAL EXAM" },
        { "POTASSIUM DEFICIENCY","METABOLIC PANEL" },
        { "CHRONIC MUSCLE CRAMP","PHYSICAL EXAM" },
        { "FIBROMYALGIA","FM/A TEST" },
        { "MUSCULAR DYSTROPHY","BIOPSY" },
        { "NECROTIZING FASCIITIS","SURGICAL EXPLORATION" },
        { "SKIN CANCER","BIOPSY"},
        { "ARTHRITIS", "X-RAY"},
        { "RICKETS","CALCIUM TEST" },
        { "OSTEOPOROSIS", "X-RAY"},
        { "FREQUENT BONE FRACTURE","X-RAY" },
        { "BONE DEATH", "BIOPSY" },
        { "LEUKEMIA", "BIOPSY" },
        { "PAGET'S", "BIOPSY" },
        { "CONSTIPATION","COLONOSCOPY" },
        { "GASTROENTERITIS" ,"PHYSICAL EXAM"},
        { "SALMONELLA INFECTION", "BLOOD" },
        { "CIRRHOSIS","CT SCAN" },
        { "HEMORRHOIDS", "RECTAL EXAM" },
        { "IRRITABLE BOWEL","SIGMOIDOSCOPY" },
        { "CROHN'S", "COLONOSCOPY"},
        { "COLON CANCER","BIOPSY" },
        { "TYPE 1 DIABETES", "METABOLIC PANEL"},
        { "TYPE 2 DIABETES","GLYCASTED HEMOGLOBIN TEST" },
        { "GALLSTONES", "CT SCAN"},
        { "PANCREATITIS","CT SCAN"},
        { "KIDNEY STONES","MRI" },
        { "KIDNEY FAILURE", "KIDNEY FUNCTION TEST" },
        { "CHRONIC KIDNEY FAILURE","BIOPSY" },
        { "LUPUS NEPHRITIS","BIOPSY" },
        { "KIDNEY INFECTION","BLOOD" },
        { "SEPSIS", "BLOOD"},
        { "URINARY INCONTINENCE","BLADDER DIARY" },
        { "KIDNEY CYST","MRI" },
        { "URINARY TRACT INFECTION","MRI" },
        { "ASTHMA","ALLERGY TEST"},
        { "PHOTOSENSITIVITY","UV TEST" },
        { "INSOMNIA","NEUROLOGICAL EXAM" },
        { "CHRONIC FATIGUE","NOCTURNAL POLYSOMNOGRAPHY" },
        { "IMMUNODEFICIENCY","BLOOD" },
        { "ADDISON'S", "ACTH"},
        { "LUPUS", "LUPUS DIFFERENTIAL DIAGNOSIS"},
        { "SARCOIDOSIS", "X-RAY"},
    };

    // Dictionary of disease - experience gain/lose on diagnosis
    Dictionary<string, int> diseaseExperience = new Dictionary<string, int>() {
        { "HYPERTENSION",5 },
        { "HIGH CHOLESTROL",10 },
        { "MALARIA",25 },
        { "JAUNDICE",20 },
        { "FLU",15 },
        { "BRONCHITIS",30 },
        { "BLOOD CLOTS", 30 },
        { "ANGINA", 25 },
        { "BRAIN ANEURYSM",100 },
        { "DEEP VEIN THROMBOSIS",110 },
        { "ARRHYTHMIA",50 },
        { "HEART ATTACK",120 },
        { "STROKE",60 },
        { "PULMONARY EMBOLISM",65 },
        { "POSTPHLEBITIC SYNDROME",40 },
        { "CHRONIC BRONCHITIS",40 },
        { "H1N1",25 },
        { "EMPHYSEMA",30 },
        { "PNEUMONIA", 35 },
        { "PNEUMOTHORAX", 25 },
        { "LUNG CANCER",115 },
        { "RESPIRATORY FAILURE", 90 },
        { "CHRONIC STRESS", 5 },
        { "NERVOUS BREAKDOWN",10 },
        { "DEMENTIA",20 },
        { "ALZHEIMER'S", 30 },
        { "BURNOUT",25 },
        { "EPILEPTIC SEIZURE",50 },
        { "MAJOR DEPRESSION",70 },
        { "PARKINSON'S",  80},
        { "HEMIPARESIS", 100 },
        { "POTASSIUM DEFICIENCY",8 },
        { "CHRONIC MUSCLE CRAMP", 5 },
        { "FIBROMYALGIA", 20 },
        { "MUSCULAR DYSTROPHY", 75 },
        { "NECROTIZING FASCIITIS",50 },
        { "SKIN CANCER", 100 },
        { "ARTHRITIS",15 },
        { "RICKETS", 20 },
        { "OSTEOPOROSIS", 30 },
        { "FREQUENT BONE FRACTURE",35 },
        { "BONE DEATH", 80 },
        { "LEUKEMIA", 90 },
        { "PAGET'S", 80 },
        { "CONSTIPATION",8 },
        { "GASTROENTERITIS", 15 },
        { "SALMONELLA INFECTION",20 },
        { "CIRRHOSIS", 30 },
        { "HEMORRHOIDS",40 },
        { "IRRITABLE BOWEL" ,45},
        { "CROHN'S",50},
        { "COLON CANCER", 85},
        { "TYPE 1 DIABETES", 60 },
        { "TYPE 2 DIABETES", 65 },
        { "GALLSTONES",55 },
        { "PANCREATITIS", 150 },
        { "KIDNEY STONES", 50 },
        { "KIDNEY FAILURE", 75 },
        { "CHRONIC KIDNEY FAILURE", 90 },
        { "LUPUS NEPHRITIS", 80 },
        { "KIDNEY INFECTION", 50 },
        { "SEPSIS", 60 },
        { "URINARY INCONTINENCE", 40 },
        { "KIDNEY CYST", 55 },
        { "URINARY TRACT INFECTION", 80 },
        { "ASTHMA", 10 },
        { "PHOTOSENSITIVITY", 15 },
        { "INSOMNIA",40 },
        { "CHRONIC FATIGUE", 45 },
        { "IMMUNODEFICIENCY", 70 },
        { "ADDISON'S", 60 },
        { "LUPUS", 70},
        { "SARCOIDOSIS", 80},
    };

    // dictionary of test - level required
    [SerializeField]
    Dictionary<string, int> levelToRunTest = new Dictionary<string, int>
    {
        { "URINE",1 },
        { "BLOOD PRESSURE TEST",3 },
        { "STETHESCOPE",2 },
        { "METABOLIC PANEL",4 },
        { "BLOOD",5 },
        { "ULTRASOUND",12 },
        { "ECG",14},
        { "MRI",20 },
        { "PULMONARY ANGIOGRAM",15 },
        { "X-RAY",10 },
        { "FLU TEST", 7 },
        { "SPIROMETER",8 },
        { "BIOPSY", 20},
        { "RESPIRATORY MONITORING", 9},
        { "EEG",2 },
        { "PSYCHOLOGICAL EVALUATION",5 },
        { "PET SCAN",7 },
        { "PSYCHIATRIC ASSESSMENT",9 },
        { "NEUROLOGICAL EXAM", 12 },
        { "PHYSICAL EXAM" ,3},
        { "FM/A TEST",10 },
        { "SURGICAL EXPLORATION", 15 },
        { "CALCIUM TEST",7 },
        { "COLONOSCOPY", 5 },
        { "CT SCAN", 18 },
        { "RECTAL EXAM", 10 },
        { "SIGMOIDOSCOPY",6 },
        { "GLYCASTED HEMOGLOBIN TEST", 8 },
        { "KIDNEY FUNCTION TEST", 17},
        { "BLADDER DIARY", 8 },
        { "ALLERGY TEST",5},
        { "UV TEST", 8 },
        { "NOCTURNAL POLYSOMNOGRAPHY", 10 },
        { "ACTH",12 },
        { "LUPUS DIFFERENTIAL DIAGNOSIS",15 },
    };

    void Awake()
    {
        UpdateDisease();
    }

    // Updates the disease according to player's level
    public void UpdateDisease()
    {
        List<string> diseaseListForCurrentLevel = new List<string>();
        foreach (string d in diseaseLibrary)
        {
            if (CanRunTest(testForDisease[d]))
            {
                diseaseListForCurrentLevel.Add(d);
            }
        }
        disease = diseaseListForCurrentLevel[Random.Range(0, diseaseListForCurrentLevel.Count)];
    }

    // Getters
    public string GetDescription()
    {
        return diseaseDecription[disease];
    }

    public string GetResult(string test)
    {
        // if diagnosis is correct player gain exp
        if(test.Equals(testForDisease[disease]))
        {
            Debug.Log(testForDisease[disease]);
            GetComponent<Experience>().GainExperience(GetDiseaseExperience()/2);
            return "The test revealed that the patient could be suffering from " + disease;
        }
        // if diagnosis is wrong player lose exp
        else
        {
            GetComponent<Experience>().GainExperience(-GetDiseaseExperience()/2);
            return "The test was a waste nothing useful could be infered from it!";
        }
    }

    public int GetLevelToRunTest(string test)
    {
        return levelToRunTest[test];
    }

    public string GetDisease()
    {
        return disease;
    }
    public int GetDiseaseExperience()
    {
        return diseaseExperience[disease];
    }

    // if test can be run for player's current level.
    public bool CanRunTest(string t)
    {
        return levelToRunTest[t] <= GetComponent<CharacterStats>().CurrentLevel() + 2;
    }


    // Saving the disease patient has
    object ISaveable.CaptureState()
    {
        return disease;
    }

    // Loading the disease patient has
    void ISaveable.RestoreState(object state)
    {
        disease = (string)state;
    }
}
