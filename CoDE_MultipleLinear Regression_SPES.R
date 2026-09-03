library(tidyverse)
library(readxl)

participants <- read_excel("C:/Users/juani/CoDE/CoDE_Participant_Dataset.xlsx")

head(participants)


names(participants)[names(participants) == "CoDE ID"] <- "CoDE_ID"
names(participants)[names(participants) == "Bortner Score"] <- "Bortner"
names(participants)[names(participants) == "Time"] <- "CompletionTime"
names(participants)[names(participants) == "Total"] <- "Total"
names(participants)[names(participants) == "Possible Action"] <- "PA"
names(participants)[names(participants) == "Self-location"] <- "SL"


#Make condition categorical
participants$Condition <- factor(participants$Condition, levels = c("Constructive", "Destructive"))

#Mean-center Bortner
participants$Bortner_c <- participants$Bortner - mean(participants$Bortner, na.rm = TRUE)

#Multiple linear Regression / moderation model
model_SL <- lm(SL ~ Condition * Bortner_c, data = participants)

summary(model_SL)
par(mfrow = c(2,2))
plot(model_SL)

#Multiple linear Regression / moderation model
model_PA <- lm(PA ~ Condition * Bortner_c, data = participants)

summary(model_PA)

par(mfrow = c(2,2))
plot(model_PA)


#Multiple linear Regression / moderation model
model_SPESTotal <- lm(Total ~ Condition * Bortner_c, data = participants)
summary(model_SPESTotal)
par(mfrow = c(2,2))
plot(model_SPESTotal)
