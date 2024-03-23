import { FinalScoreDto } from "../FinalScore/FinalScoreDto";
import { DecisionDto } from "./DecisionDto";

export interface DecisionScoreDto {
  decision: DecisionDto;
  finalScore: FinalScoreDto;
}
