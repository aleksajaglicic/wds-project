export enum WorkoutType {
    Plank = "Plank",
    PushUp = "PushUp",
    PullUp = "PullUp",
    SitUp = "SitUp",
    ChinUp = "ChinUp"
}

export interface Workout {
    id: string;
    userId: string;
    tiredness: number;
    difficulty: number;
    caloriesSpent: number;
    duration: number;
    workoutDate: string;
    workoutType: WorkoutType;
    additionalNote: string;
}