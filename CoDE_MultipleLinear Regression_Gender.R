library(tidyverse)
library(readxl)

participants <- read_excel("C:/Users/juani/CoDE/CoDE_Participant_Dataset.xlsx")

head(participants)

#Rename columns because of space

names(participants)[names(participants) == "CoDE ID"] <- "CoDE_ID"
names(participants)[names(participants) == "Bortner Score"] <- "Bortner"
names(participants)[names(participants) == "Time"] <- "CompletionTime"
names(participants)[names(participants) == "LeftHand_Dist"] <- "LeftHand_Dist"
names(participants)[names(participants) == "RightHand_Dist"] <- "RightHand_Dist"
names(participants)[names(participants) == "Head_Dist"] <- "Head_Dist"
names(participants)[names(participants) == "Total Movement"] <- "Total"
names(participants)[names(participants) == "Gender"] <- "Gender"

#Make condition categorical
participants$Condition <- factor(participants$Condition, levels = c("Constructive", "Destructive"))

#Mean-center Bortner
participants$Bortner_c <- participants$Bortner - mean(participants$Bortner, na.rm = TRUE)

#Multiple linear Regression / moderation model
#model <- lm(CompletionTime ~ Condition * Bortner_c, data = participants)

#Results
#summary(model)

# ------------------------------
# GENDER
# ------------------------------

#MAKE GENDER CATEGORICAL
participants$Gender <- factor(participants$Gender)

table(participants$Gender)
table(participants$Gender, participants$Condition)

participants$Gender <- relevel(
  participants$Gender,
  ref = "M"
)

model_completion_gender <- lm(
  Total ~ Condition * Bortner_c + Gender,
  data = participants
)

summary(model_completion_gender)

anova(
  model,
  model_completion_gender
)

#Multiple linear Regression / moderation model
model_Gender <- lm(RightHand ~ Condition * Bortner_c, data = participants)
summary(model_Gender)
