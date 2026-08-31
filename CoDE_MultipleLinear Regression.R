#Multiple Linear Regression

library(tidyverse)



library(readxl)
participants <- read_excel("C:/Users/juani/CoDE/CoDE_Participant_Dataset.xlsx")

head(participants)

#Rename columns because of space

names(participants)[names(participants) == "CoDE ID"] <- "CoDE_ID"
names(participants)[names(participants) == "Bortner Score"] <- "Bortner"
names(participants)[names(participants) == "Time"] <- "CompletionTime"


#Make condition categorical
participants$Condition <- factor(participants$Condition, levels = c("Constructive", "Destructive"))

#Mean-center Bortner
participants$Bortner_c <- participants$Bortner - mean(participants$Bortner, na.rm = TRUE)

#Multiple linear Regression / moderation model
model <- lm(CompletionTime ~ Condition * Bortner_c, data = participants)

#Results
summary(model)

#Destructive simple-slopes
participants$Condition <- relevel(
  participants$Condition, 
  ref = "Destructive"
)

model_destructive <- lm(
  CompletionTime ~ Condition * Bortner_c,
  data = participants
)

summary(model_destructive)

#PLOT
library(ggplot2)

ggplot(participants, aes(
  x = Bortner,
  y = CompletionTime,
  color = Condition
  )
) +
  geom_point(size = 3, alpha = 0.7) +
  geom_smooth(
    method = "lm",
    se = TRUE
  ) + 
  labs(
    title = "Relationship Between Bortner Score and Completion Time",
    subtitle = "Constructive vs. Destructive Task Condition",
    x = "Bortner Score",
    y = "Completion Time (seconds)",
    color = "Task Condition"
  )+
  theme_classic(base_size = 16)

#Second plot
plot(
  participants$Bortner,
  participants$CompletionTime,
  xlab = "Bortner Score",
  ylab = "Completion Time (seconds)",
  main = "Bortner Score and Completion Time by Task",
  pch = 19
)

# Constructive regression line
abline(
  lm(CompletionTime ~ Bortner,
     data = participants,
     subset = Condition == "Constructive"),
  lty = 1,
  lwd = 2
)

# Destructive regression line
abline(
  lm(CompletionTime ~ Bortner,
     data = participants,
     subset = Condition == "Destructive"),
  lty = 2,
  lwd = 2
)

legend(
  "topright",
  legend = c("Constructive", "Destructive"),
  lty = c(1, 2),
  lwd = 2
)


