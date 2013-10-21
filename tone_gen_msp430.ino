int state[] = {
  0, 0};
int last[] = {
  0, 0};
int cur, note, pin, numNotes;
int time[][2] = {
  {
    1000 / 880, 1000 / 600  }
  ,
  {
    1000 / 440, 1000 / 523  }
  ,
  {
    1000 / 220, 1000 / 165  }
  ,
  {
    1000 / 110, 1000 / 98  }
  ,
  {
    1000 / 55, 1000 / 73  }
};
int pins[] = {
  P1_0, P1_4};
void setup()
{
  pinMode(P1_0, OUTPUT);
  pinMode(P1_4, OUTPUT);
  pinMode(P1_1, INPUT);
  pinMode(P1_2, INPUT);
  pinMode(P1_3, INPUT);
  numNotes = 5;
  last[0] = last[1] = millis();
  note = 0;
}

void loop()
{
  if(digitalRead(P1_1) == 0)
  {
    note = (note + 1);
    delay(500);
  }
  else if(digitalRead(P1_2) == 0)
  {
    note = (note - 1);
    delay(500);
  }
  if(note >= numNotes)
    note = 0;
  else if(note < 0)
    note = numNotes - 1;
  if(digitalRead(P1_3) == 0)
  {
    cur = millis();
    for(int i = 0; i < 2; ++i)
    {
      if(cur - last[i] > time[note][i])
      {
        state[i] = 1 - state[i];
        digitalWrite(pins[i], state[i]);
        last[i] = cur;
      }
    }
  }
}




