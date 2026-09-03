#Multiple Linear Regression

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
model <- lm(CompletionTime ~ Condition * Bortner_c, data = participants)

#Results
summary(model)

# -----------------------------------
#Use for changing condition reference
# -----------------------------------
#participants$Condition <- relevel(
  #participants$Condition, 
  #ref = "Destructive"
#)

#Re-run model after


# ------------------------------
# Head Movement
# ------------------------------

head(participants)

model_head <- lm(Head_Dist ~ Condition * Bortner_c, data = participants)


# -------------------------------
# LeftHand Movement
# -------------------------------
model_LeftHand <- lm(LeftHand_Dist ~ Condition * Bortner_c, data = participants)

summary(model_LeftHand)




# -------------------------------
# Right hand Movement
# ------------------------------

model_RightHand <- lm(RightHand_Dist ~ Condition * Bortner_c, data = participants)

summary(model_RightHand)



# -------------------------------
# Right hand Movement
# ------------------------------
model_LM<- lm(Total ~ Condition * Bortner_c, data = participants)
summary(model_LM)


model_LM<- lm(Total ~ Condition * Bortner_c, data = participants)
summary(model_LM)
# -------------------------------
# POST-HOC
# --------------------------------

#Destructive simple-slopes
participants$Condition <- relevel(
  participants$Condition, 
  ref = "Constructive"
)

model_destructive <- lm(
  CompletionTime ~ Condition * Bortner_c,
  data = participants
)

summary(model_destructive)


participants$Condition <- relevel(
  participants$Condition,
  ref = "Constructive"
)

model_constructive <- lm(
  CompletionTime ~ Condition * Bortner_c,
  data = participants
)

constructive_results <- summary(model_constructive)$coefficients
print(
  constructive_results["Bortner_c", , drop = FALSE]
)

participants$Condition <- relevel(
  participants$Condition,
  ref = "Destructive"
)

model_destructive <- lm(
  CompletionTime ~ Condition * Bortner_c,
  data = participants
)

destructive_results <- summary(model_destructive)$coefficients

print(
  destructive_results["Bortner_c", , drop = FALSE]
)



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
  theme_classic(base_size = 14)



library(ggplot2)

ggplot(
  participants,
  aes(
    x = Bortner,
    y = CompletionTime,
    color = Condition,
    linetype = Condition
  )
) +
  geom_point(
    size = 3,
    alpha = 0.7
  ) +
  geom_smooth(
    method = "lm",
    se = TRUE,
    fullrange = TRUE,
    linewidth = 1
  ) +
  scale_linetype_manual(
    values = c(
      "Constructive" = "solid",
      "Destructive" = "solid"
    )
  ) +
  labs(
    title = "Relationship Between Bortner Score and Completion Time",
    subtitle = "Constructive vs. Destructive Task Condition",
    x = "Bortner Score",
    y = "Completion Time (seconds)",
    color = "Task Condition",
    linetype = "Task Condition"
  ) +
  theme_classic(base_size = 14)



# -----------------------
#Second plot
# -----------------------
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

# ------------------------
# Colored plot
# ------------------------

# Colors for each condition
constructive_color <- "#14a416"
destructive_color <- "#1b73a6"

# Plot points
plot(
  participants$Bortner,
  participants$CompletionTime,
  type = "n",
  xlab = "Bortner Score",
  ylab = "Completion Time (seconds)",
  main = ""
  #Bortner Score and Completion Time by Task
)

# Constructive points
points(
  participants$Bortner[participants$Condition == "Constructive"],
  participants$CompletionTime[participants$Condition == "Constructive"],
  pch = 19,
  col = constructive_color
)

# Destructive points
points(
  participants$Bortner[participants$Condition == "Destructive"],
  participants$CompletionTime[participants$Condition == "Destructive"],
  pch = 19,
  col = destructive_color
)

# Constructive regression line
abline(
  lm(
    CompletionTime ~ Bortner,
    data = participants,
    subset = Condition == "Constructive"
  ),
  col = constructive_color,
  lwd = 2,
  lty = 1
)

# Destructive regression line
abline(
  lm(
    CompletionTime ~ Bortner,
    data = participants,
    subset = Condition == "Destructive"
  ),
  col = destructive_color,
  lwd = 2,
  lty = 2
)

# Legend
legend(
  "topright",
  legend = c("Constructive", "Destructive"),
  col = c(constructive_color, destructive_color),
  pch = 19,
  lty = c(1, 2),
  lwd = 2
)


# ------------------
# Plot for Movement
# ------------------

# Head
plot(
  participants$Bortner,
  participants$Head_Dist,
  type = "n",
  xlab = "Bortner Score",
  ylab = "Head Distance",
  main = ""
)

# Constructive points
points(
  participants$Bortner[participants$Condition == "Constructive"],
  participants$Head_Dist[participants$Condition == "Constructive"],
  pch = 19,
  col = constructive_color
)

# Destructive points
points(
  participants$Bortner[participants$Condition == "Destructive"],
  participants$Head_Dist[participants$Condition == "Destructive"],
  pch = 19,
  col = destructive_color
)

# Constructive regression line
abline(
  lm(
    Head_Dist ~ Bortner,
    data = participants,
    subset = Condition == "Constructive"
  ),
  col = constructive_color,
  lwd = 2,
  lty = 1
)

# Destructive regression line
abline(
  lm(
    Head_Dist ~ Bortner,
    data = participants,
    subset = Condition == "Destructive"
  ),
  col = destructive_color,
  lwd = 2,
  lty = 2
)

legend(
  "topright",
  legend = c("Constructive", "Destructive"),
  col = c(constructive_color, destructive_color),
  pch = 19,
  lty = c(1, 2),
  lwd = 2
)

# ------------------------------
# LEFT HAND
# -----------------------------
plot(
  participants$Bortner,
  participants$LeftHand_Dist,
  type = "n",
  xlab = "Bortner Score",
  ylab = "Left-Hand Distance",
  main = ""
)

points(
  participants$Bortner[participants$Condition == "Constructive"],
  participants$LeftHand_Dist[participants$Condition == "Constructive"],
  pch = 19,
  col = constructive_color
)

points(
  participants$Bortner[participants$Condition == "Destructive"],
  participants$LeftHand_Dist[participants$Condition == "Destructive"],
  pch = 19,
  col = destructive_color
)

abline(
  lm(
    LeftHand_Dist ~ Bortner,
    data = participants,
    subset = Condition == "Constructive"
  ),
  col = constructive_color,
  lwd = 2,
  lty = 1
)

abline(
  lm(
    LeftHand_Dist ~ Bortner,
    data = participants,
    subset = Condition == "Destructive"
  ),
  col = destructive_color,
  lwd = 2,
  lty = 2
)

legend(
  "topright",
  legend = c("Constructive", "Destructive"),
  col = c(constructive_color, destructive_color),
  pch = 19,
  lty = c(1, 2),
  lwd = 2
)

# ------------------------------
# RIGHT HAND
# -----------------------------
plot(
  participants$Bortner,
  participants$RightHand_Dist,
  type = "n",
  xlab = "Bortner Score",
  ylab = "Right-Hand Distance",
  main = ""
)

points(
  participants$Bortner[participants$Condition == "Constructive"],
  participants$RightHand_Dist[participants$Condition == "Constructive"],
  pch = 19,
  col = constructive_color
)

points(
  participants$Bortner[participants$Condition == "Destructive"],
  participants$RightHand_Dist[participants$Condition == "Destructive"],
  pch = 19,
  col = destructive_color
)

abline(
  lm(
    RightHand_Dist ~ Bortner,
    data = participants,
    subset = Condition == "Constructive"
  ),
  col = constructive_color,
  lwd = 2,
  lty = 1
)

abline(
  lm(
    RightHand_Dist ~ Bortner,
    data = participants,
    subset = Condition == "Destructive"
  ),
  col = destructive_color,
  lwd = 2,
  lty = 2
)

legend(
  "topright",
  legend = c("Constructive", "Destructive"),
  col = c(constructive_color, destructive_color),
  pch = 19,
  lty = c(1, 2),
  lwd = 2
)

# -----------------------------------\
# Total Movement
# -----------------------------------
plot(
  participants$Bortner,
  participants$Total,
  type = "n",
  xlab = "Bortner Score",
  ylab = "Total Movement",
  main = ""
)

points(
  participants$Bortner[participants$Condition == "Constructive"],
  participants$Total[participants$Condition == "Constructive"],
  pch = 19,
  col = constructive_color
)

points(
  participants$Bortner[participants$Condition == "Destructive"],
  participants$Total[participants$Condition == "Destructive"],
  pch = 19,
  col = destructive_color
)

abline(
  lm(
    Total ~ Bortner,
    data = participants,
    subset = Condition == "Constructive"
  ),
  col = constructive_color,
  lwd = 2,
  lty = 1
)

abline(
  lm(
    Total ~ Bortner,
    data = participants,
    subset = Condition == "Destructive"
  ),
  col = destructive_color,
  lwd = 2,
  lty = 2
)

legend(
  "topright",
  legend = c("Constructive", "Destructive"),
  col = c(constructive_color, destructive_color),
  pch = 19,
  lty = c(1, 2),
  lwd = 2
)

